using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Angular_Fixed : Shooter_Base<AngularBulletData> {

    protected override void Shoot(AngularBulletData data)
    {
        Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(data.degree * Mathf.Deg2Rad), Mathf.Sin(data.degree * Mathf.Deg2Rad)) * data.speed;
    }

}
