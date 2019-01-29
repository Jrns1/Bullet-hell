using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Linear : Shooter_Base<LinearBulletData> {

    protected override void Shoot(LinearBulletData data)
    {
        Bullet.GetComponent<Rigidbody2D>().velocity = (GameManager.Ins.player.position - transform.position).normalized * data.speed;
    }

}
