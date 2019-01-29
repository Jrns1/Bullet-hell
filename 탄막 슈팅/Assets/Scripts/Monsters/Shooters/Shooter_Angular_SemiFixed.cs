using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Angular_SemiFixed : Shooter_Base<AngularBulletData> {

    Vector2 unnormalizedDir;

    public override void TriggerPattern()
    {
        unnormalizedDir = GameManager.Ins.player.position - transform.position;
        base.TriggerPattern();
    }

    protected override void Shoot(AngularBulletData data)
    {
        Bullet.GetComponent<Rigidbody2D>().velocity = GameManager.Ins.RotateDirection(unnormalizedDir, data.degree) * data.speed;
    }

}
