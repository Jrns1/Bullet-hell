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

    protected override float GetBulletDelay(int index)
    {
        return pattern[index].delay;
    }

    protected override void Shoot(int index)
    {
        CircularBulletData data = pattern[index];
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = GameManager.Instance.RotateDirection(GameManager.Instance.player.position - transform.position, data.degree) * data.speed;
    }

}
