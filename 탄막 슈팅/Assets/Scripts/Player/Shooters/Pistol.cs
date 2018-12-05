using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : PlayerShooter {

    public float bulletDamage;
    public float bulletSpeed;
    public float bulletKnockback;
    public float delay;

	protected override void Update() {
        base.Update();

        if (Input.GetMouseButtonDown(0) && magazine > 0)
        {
            GameObject bulletObj = GetBullet("Bullet_Player", muzzle.position);
            bulletObj.GetComponent<Rigidbody2D>().velocity = aim * bulletSpeed;

            Bullet_Player bullet = bulletObj.GetComponent<Bullet_Player>();
            bullet.damage = bulletDamage;
            bullet.knockbackForce = aim * bulletKnockback;

            magazine--;
        }
	}
}
