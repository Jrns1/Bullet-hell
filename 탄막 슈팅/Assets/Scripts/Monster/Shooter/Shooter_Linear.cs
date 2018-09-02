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

    public override float Time
    {
        get
        {
            float t = 0;
            for (int i = 0; i < Length; i++)
            {
                t += pattern[i].delay;
            }

            return t;
        }
    }

    protected override IBulletData GetBulletData(int index)
    {
        return pattern[index];
    }

    protected override void Shoot(IBulletData bulletData)
    {
        LinearBulletData data = (LinearBulletData)bulletData;
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = (GameManager.Instance.player.position - transform.position).normalized * data.speed;
    }

}
