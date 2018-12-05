using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class PlayerShooter : MonoBehaviour {

    public Transform muzzle;
    public int magazineSize;
    public float reloadTime;

    public delegate void ReloadDelegate();
    public event ReloadDelegate ReloadEvent;

    protected int magazine;
    protected Vector2 aim;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        magazine = magazineSize;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main;
    }

    protected virtual void Update()
    {
        // 조준
        aim = ((Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
        float aimDegree = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;

        if (-90 <= aimDegree && aimDegree < 90)
            transform.rotation = Quaternion.Euler(0, 0, aimDegree);
        else
            transform.rotation = Quaternion.Euler(0, 180, 180 - aimDegree);

        // 재장전
        if (Input.GetKeyDown(KeyCode.R) && magazine < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    protected GameObject GetBullet(string name, Vector2 pos)
    {
        return ObjectPool.Instance.PopFromPool(name, pos);
    }

    IEnumerator Reload()
    {
        if (ReloadEvent != null)
            ReloadEvent();
        
        AudioManager.Instance.PlaySoundEffect(name + " Reload");
        yield return new WaitForSeconds(reloadTime);
        magazine = magazineSize;
    }

}
