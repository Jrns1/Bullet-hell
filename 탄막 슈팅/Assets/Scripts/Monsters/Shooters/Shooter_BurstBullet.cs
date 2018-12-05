using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_BurstBullet : Shooter_Base {

    public BurstBulletData[] pattern;

    protected override int Length
    {
        get
        {
            return pattern.Length;
        }
    }
    
    protected override float GetBulletDelay(int index)
    {
        return pattern[index].delay;
    }

    protected override void Shoot(int index)
    {
        BurstBulletData data = pattern[index];
        GameObject bullet = ObjectPool.Instance.PopFromPool("Burst Bullet", transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = (GameManager.Instance.player.position - transform.position).normalized * data.speed;
        Bullet_Burst bulletScript = bullet.GetComponent<Bullet_Burst>();
        bulletScript.cnt = data.bulletCount;
        bulletScript.burstSec = data.burstSecond;
        bulletScript.bulletName = bulletName;
        bulletScript.speed = data.burstBulletSpeed;
    }


}
