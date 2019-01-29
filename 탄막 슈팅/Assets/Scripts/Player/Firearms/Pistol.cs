using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Firearm {

	protected override void Shoot() {
        if (Input.GetMouseButtonDown(0) && magazine > 0 && isShootable)
        {
            ShootBullet("Bullet_Player");

            base.Shoot();
        }
	}
}
