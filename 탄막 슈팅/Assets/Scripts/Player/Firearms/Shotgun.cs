using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Firearm {

    public float bulletCnt;
    public float speedVariation;

    protected override void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && magazine > 0 && isShootable)
        {
            for (int i = 0; i < bulletCnt; i++)
            {
                ShootBullet("Bullet_Player").GetComponent<Rigidbody2D>().velocity += Random.insideUnitCircle * speedVariation;
            }

            base.Shoot();
        }
    }

}
