using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Linear : Shooter_WaitUntilSee {

    public LinearBulletData[] pattern;

    protected override int Length
    {
        get
        {
            return pattern.Length;
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
