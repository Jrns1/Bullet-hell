using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Angular : Shooter_Base<AngularBulletData> {

    protected override void Shoot(AngularBulletData data)
    {
        Bullet.GetComponent<Rigidbody2D>().velocity = GameManager.Ins.RotateDirection(GameManager.Ins.player.position - transform.position, data.degree) * data.speed;
    }

}
