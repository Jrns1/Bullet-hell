using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : Singleton<GameManager> {

    public Transform player;
    public bool isActionInhibited;
    public Image panel;
    public string sceneEntryPortalName;
    public SavePointData savePoint;

    [HideInInspector] public int layerMask_Wall;

    [HideInInspector] public bool isPauseAllowed = true;
    [HideInInspector] public bool isInteractionAllowed = true;

    public const float FADING_TIME = .5f;

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
            GameManager.Ins.isActionInhibited = !pause;

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
        yield return Fader.Ins.Fade(panel, FADING_TIME, 0, 1);
        if (sceneName != null)
            SceneManager.LoadScene(sceneName);
    }

    #region Functions
    public static readonly Func<float, float> Sqr = (x) => x * x;
    
    public Coroutine Delay(Action action, float t = 0)
    {
        return StartCoroutine(DelayCo(action, t));
    }

    IEnumerator DelayCo(Action action, float t)
    {
        yield return new WaitForSeconds(t);
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
        ObjectPool.Ins.PushToPool(gameObject);
    }

    public WaitUntil WaitForAnimation(Animator animator, string animation, Func<float, bool> waitAnimation)
    {
        return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animation) && waitAnimation(animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
    }

    public Vector2 Arc(Vector2 initial, Vector2 final, float h, float s)
    {
        return s * final + (1 - s) * initial + new Vector2(0, 1) * 4 * h * s * (1 - s);
    }

    public void Particle(string name, Vector2 position, bool active = true, Transform parent = null)
    {
        GameObject gameObject = ObjectPool.Ins.PopFromPool(name, position, active, parent);
        ParticleSystem particle = gameObject.GetComponent<ParticleSystem>();
        Ins.StartCoroutine(Remover(gameObject, () => particle.isStopped));
    }
    #endregion

}
