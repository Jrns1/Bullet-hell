using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeGun : Firearm {

    public float chargeSec;

    bool isFullCharged;

    protected override void Shoot()
    {
        if (magazine > 0 && isShootable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Charge());
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopCoroutine(Charge());

                if (isFullCharged)
                {
                    ShootBullet("Bullet_Player");

                    base.Shoot();
                }
            }
        }
    }

    IEnumerator Charge()
    {
        isFullCharged = false;
        yield return new WaitForSeconds(chargeSec);
        isFullCharged = true;
    }
}
