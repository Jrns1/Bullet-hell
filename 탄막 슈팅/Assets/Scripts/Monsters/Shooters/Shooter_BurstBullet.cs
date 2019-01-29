using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_BurstBullet : Shooter_Base<BurstBulletData> {

    protected override void Shoot(BurstBulletData data)
    {
        GameObject bullet = Bullet;
        bullet.GetComponent<Rigidbody2D>().velocity = (GameManager.Ins.player.position - transform.position).normalized * data.speed;
        Bullet_Burst bulletScript = bullet.GetComponent<Bullet_Burst>();
        bulletScript.cnt = data.bulletCount;
        bulletScript.burstSec = data.burstSecond;
        bulletScript.bulletName = bulletName;
        bulletScript.speed = data.burstBulletSpeed;
    }


}
