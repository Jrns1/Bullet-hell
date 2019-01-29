using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWhenSee : MonoBehaviour {

    public IPattern shooter;
    public float sqrSight = 400f; // 20 * 20

    const float FINDING_DELAY = .1f;


    private void OnEnable()
    {
        StartCoroutine(WaitUntilSeeTarget());
    }


    IEnumerator WaitUntilSeeTarget()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitUntil(CanSeeTarget);
            GameManager.Ins.Delay(() => shooter.isActive = true, 1f);
            yield return new WaitWhile(CanSeeTarget);
            shooter.isActive = false;
        }
    }

    bool CanSeeTarget()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, GameManager.Ins.player.position - transform.position, Vector2.Distance(GameManager.Ins.player.position, transform.position), GameManager.Ins.layerMask_Wall);
        if (!ray.collider && (GameManager.Ins.player.position - transform.position).sqrMagnitude < sqrSight)
            return true;
        return false;
    }

}
