using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Spraying : Shooter_Base<LinearBulletData> {

    public float spread;

    protected override void Shoot(LinearBulletData data)
    {
        Bullet.GetComponent<Rigidbody2D>().velocity = GameManager.Ins.RotateDirection(GameManager.Ins.player.position - transform.position, Random.Range(-spread, spread)) * data.speed;
    }
}
