using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class PlayerShooter : MonoBehaviour {

    public string bulletName;
    public float bulletSpeed;
    public int magazineSize;
    public float reloadTime;
    public float shootingDelay_Auto;
    public float shootingDelay_Semiauto;

    bool mouseDown;
    float shotTime = 0f;
    int magazine;
    Action Shoot;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        Shoot = ShootNormal;
        magazine = magazineSize;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main;
    }

    void Update () {
        // 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        // 발사
        float delay;
        if (Input.GetMouseButtonDown(0))
            delay = shootingDelay_Semiauto;
        else if (Input.GetMouseButton(0))
            delay = shootingDelay_Auto;
        else
            return;

        if (Time.time > shotTime + delay && magazine > 0)
        {
            Shoot();
            shotTime = Time.time;
            magazine--;
        }
	}

    IEnumerator Reload()
    {
        AudioManager.Instance.PlaySoundEffect("TestGun Reload");
        yield return new WaitForSeconds(reloadTime);
        magazine = magazineSize;
    }

    void ShootNormal()
    {
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = ((Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized * bulletSpeed;
    }
}
