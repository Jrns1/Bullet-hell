using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_FixedCircular : Shooter_Circular {

    protected override void Shoot(int index)
    {
        CircularBulletData data = pattern[index];
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(data.degree * Mathf.Deg2Rad), Mathf.Sin(data.degree * Mathf.Deg2Rad)) * data.speed;
    }

}
