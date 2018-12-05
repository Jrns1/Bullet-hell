using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Burst : Bullet_Base {

    public string bulletName;
    public int cnt;
    public float speed;
    public float burstSec;


    private void OnEnable()
    {
        StartCoroutine(BurstTimer());
    }

    IEnumerator BurstTimer()
    {
        yield return null; // Wait for setting variables
        yield return new WaitForSeconds(burstSec);
        Burst();
    }

    public override void OnCollisionWithTarget()
    {
        Burst();
    }

    protected override void OnCollisionWithWall(Collision2D collision)
    {
        Burst();
    }

    void Burst()
    {
        Vector2 direction = (GameManager.Instance.player.position - transform.position).normalized;
        float degreePerBullet = 360 / cnt;
        for (float degree = 0; degree < 360; degree += degreePerBullet)
        {
            GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
            bullet.GetComponent<Rigidbody2D>().velocity = GameManager.Instance.RotateDirection(direction, degree) * speed;
        }

        ObjectPool.Instance.PushToPool(gameObject);
    }

}
