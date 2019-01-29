using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class Firearm : MonoBehaviour {

    public float bulletDamage;
    public float bulletSpeed;
    public float bulletKnockback;
    public float spread;
    public float shootingDelay;
    public float reloadTime;
    public int magazineSize;
    public float disposingSec;

    public SpriteRenderer firearm;
    public Transform muzzle;
    public Animator fireEffect;
    public GameObject sideArm;

    protected int magazine;
    protected Vector2 aim;
    protected bool isShootable = true; 

    Camera cam;
    Coroutine shooting;

    private void Awake()
    {
        cam = Camera.main;
        magazine = magazineSize;
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (firearm.enabled)
            shooting = StartCoroutine(ShootingState());
    }

    private void Start()
    {
        FirearmManager.Ins.UpdateMagazine(magazine);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main;
    } 

    private void Update()
    {
        // 조준
        aim = ((Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
        float aimDegree = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;

        if (-90 <= aimDegree && aimDegree < 90)
            transform.rotation = Quaternion.Euler(0, 0, aimDegree);
        else
            transform.rotation = Quaternion.Euler(0, 180, 180 - aimDegree);

        if (Input.GetKeyDown(KeyCode.LeftControl) && !GameManager.Ins.isActionInhibited)
        {
            ChangeArm();
        }
    }

    void ChangeArm()
    {
        if (firearm.enabled) // deactivate
        {
            firearm.enabled = false;
            StopCoroutine(shooting);
        }
        else // activate
        {
            firearm.enabled = true;
            shooting = StartCoroutine(ShootingState());
        }
    }

    IEnumerator ShootingState()
    {
        while (true)
        {
            if (GameManager.Ins.isActionInhibited)
                continue;

            if (Input.GetKeyDown(KeyCode.R) && magazine < magazineSize)
            {
                StartCoroutine(Reload());
            }

            Shoot();

            yield return null;
        }
    }

    protected virtual void Shoot()
    {
        magazine--;
        FirearmManager.Ins.UpdateMagazine(magazine);
        StartCoroutine(DelayShooting());
        //StartCoroutine(FireEffect());
    }

    protected GameObject ShootBullet(string name)
    {
        Vector2 dir = GameManager.Ins.RotateDirection(aim, UnityEngine.Random.Range(-spread, spread));

        GameObject bulletObj = ObjectPool.Ins.PopFromPool(name, muzzle.position);
        bulletObj.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;

        Bullet_Player bullet = bulletObj.GetComponent<Bullet_Player>();
        bullet.damage = bulletDamage;
        bullet.knockbackForce = dir * bulletKnockback;
        bullet.disposeSec = disposingSec;

        return bulletObj;
    }

    IEnumerator FireEffect()
    {
        fireEffect.Play("Shoot");
        yield return new WaitForSeconds(.2f);
    }

    IEnumerator DelayShooting()
    {
        isShootable = false;
        yield return new WaitForSeconds(shootingDelay);
        isShootable = true;
    }

    IEnumerator Reload()
    {
        magazine = 0;
        AudioManager.Ins.PlaySoundEffect(name + " Reload");

        FirearmManager.Ins.reloading.ShowReloadUI(reloadTime);
        yield return new WaitForSeconds(reloadTime);

        magazine = magazineSize;
        FirearmManager.Ins.UpdateMagazine(magazine);
    }

}
