using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Spraying : Shooter_Linear {

    public float spread;

    protected override void Shoot(int index)
    {
        LinearBulletData data = pattern[index];
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = GameManager.Instance.RotateDirection(GameManager.Instance.player.position - transform.position, Random.Range(-spread, spread)) * data.speed;
    }
}
