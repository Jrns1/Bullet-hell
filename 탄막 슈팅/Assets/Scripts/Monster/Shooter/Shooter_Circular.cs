using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Circular : Shooter_WaitUntilSee {

    public CircularBulletData[] pattern;

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
        CircularBulletData data = (CircularBulletData)bulletData;
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = GameManager.RotateVector(GameManager.Instance.player.position - transform.position, data.degree).normalized * data.speed;
    }

}
