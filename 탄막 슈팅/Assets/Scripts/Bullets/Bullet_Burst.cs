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
        GameManager.Ins.Delay(Burst, burstSec);
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
        Vector2 direction = (GameManager.Ins.player.position - transform.position).normalized;
        float degreePerBullet = 360 / cnt;
        for (float degree = 0; degree < 360; degree += degreePerBullet)
        {
            GameObject bullet = ObjectPool.Ins.PopFromPool(bulletName, transform.position);
            bullet.GetComponent<Rigidbody2D>().velocity = GameManager.Ins.RotateDirection(direction, degree) * speed;
        }

        Kill();
    }

}
