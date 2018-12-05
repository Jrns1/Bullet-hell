using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWhenSee : MonoBehaviour {

    const float SIGHT = 20f;
    const float FINDING_DELAY = .1f;

    Shooter_Base shooter;


    private void Awake()
    {
        shooter = GetComponent<Shooter_Base>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitUntilSeeTarget());
    }


    IEnumerator WaitUntilSeeTarget()
    {
        while (isActiveAndEnabled)
        {
            shooter.isShooting = CanSeeTarget();
            yield return new WaitForSeconds(FINDING_DELAY);
        }
    }

    bool CanSeeTarget()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, GameManager.Instance.player.position - transform.position, Vector2.Distance(GameManager.Instance.player.position, transform.position), GameManager.Instance.layerMask_Wall);
        if (!ray.collider && GameManager.IsNear(GameManager.Instance.player.position, transform.position, SIGHT))
            return true;
        return false;
    }

}
