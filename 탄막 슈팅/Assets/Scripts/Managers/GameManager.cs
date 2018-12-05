using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : Singleton<GameManager> {

    public Transform player;
    public Image panel;
    public string sceneEntryPortalName;
    public SavePointData savePoint;

    [HideInInspector] public int layerMask_Wall;
    public const float FADING_TIME = .5f;

    [HideInInspector] public bool isPauseAllowed = true;
    [HideInInspector] public bool isInteractionAllowed = true;

    bool pause = false;


    private void Awake()
    {
        layerMask_Wall = 1 << LayerMask.NameToLayer("Lower Wall");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPauseAllowed)
        {
            pause = !pause;
            player.GetComponent<PlayerMovement>().isAllowedToMove = !pause;

            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public IEnumerator EnterScene(string sceneName)
    {
        yield return Fader.Instance .Fade(Instance.panel, FADING_TIME);
        if (sceneName != null)
            SceneManager.LoadScene(sceneName);
    }

    #region Functions
    public IEnumerator Delay(Action action)
    {
        yield return null;
        action();
    }

    public Vector2 RotateDirection(Vector2 unnormalizedV2, float degree)
    {
        float desiredRad = Mathf.Atan2(unnormalizedV2.y, unnormalizedV2.x) - degree * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(desiredRad), Mathf.Sin(desiredRad));
    }

    public IEnumerator Remover(GameObject gameObject, Func<bool> Predicate)
    {
        yield return new WaitUntil(Predicate);
        ObjectPool.Instance.PushToPool(gameObject);
    }

    public WaitUntil WaitForAnimation(Animator animator, string animation, Func<float, bool> func)
    {
        return new WaitUntil(() => (func(animator.GetCurrentAnimatorStateInfo(0).normalizedTime) && animator.GetCurrentAnimatorStateInfo(0).IsName(animation)));
    }

    public Vector2 Arc(Vector2 initial, Vector2 final, float h, float s)
    {
        return s * final + (1 - s) * initial + new Vector2(0, 1) * 4 * h * s * (1 - s);
    }

    public void Particle(string name, Vector2 position, bool active = true, Transform parent = null)
    {
        GameObject gameObject = ObjectPool.Instance.PopFromPool(name, position, active, parent);
        ParticleSystem particle = gameObject.GetComponent<ParticleSystem>();
        Instance.StartCoroutine(Remover(gameObject, () => particle.isStopped));
    }
    #endregion

    public static bool IsNear(Vector2 a, Vector2 b, float distance)
    {
        Vector2 diff = a - b;
        return diff.x * diff.x + diff.y * diff.y <= distance * distance;
    }

}
