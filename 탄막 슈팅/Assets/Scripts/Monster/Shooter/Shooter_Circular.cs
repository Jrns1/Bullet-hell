using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Circular : Shooter_Base {

    public CircularBulletData[] pattern;

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
        CircularBulletData data = (CircularBulletData)bulletData;
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = GameManager.RotateVector(GameManager.Instance.player.position - transform.position, data.degree).normalized * data.speed;
    }

}
