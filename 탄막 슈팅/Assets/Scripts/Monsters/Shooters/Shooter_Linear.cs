using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Linear : Shooter_Base {

    public LinearBulletData[] pattern;

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
        LinearBulletData data = pattern[index];
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = (GameManager.Instance.player.position - transform.position).normalized * data.speed;
    }

}
