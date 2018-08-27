using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_WaitUntilSee : Shooter_Base {

    const float sightRange = 20f;
    const float findingDelay = .1f;

    public override IEnumerator Attack()
    {
        for (int i = 0; i < Length && isShooting; i++)
        {
            int last = i > 0 ? i - 1 : Length - 1;
            if (GetBulletData(last).Delay > 0)
                yield return StartCoroutine(WaitUntilSeeTarget());

            IBulletData data = GetBulletData(i);
            Shoot(data);

            if (data.Delay > 0)
                yield return new WaitForSeconds(data.Delay);
        }
    }

    IEnumerator WaitUntilSeeTarget()
    {
        while (true)
        {
            if (CanSeeTarget())
                yield break;
            yield return new WaitForSeconds(findingDelay);
        }
    }

    bool CanSeeTarget()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, GameManager.Instance.player.position - transform.position, sightRange, GameManager.Instance.layerMask_Wall_Player);
        if (ray && ray.collider.CompareTag("Player"))
            return true;
        return false;
    }

}
