using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWhenSee : MonoBehaviour {

    const float sightRange = 20f;
    const float findingDelay = .1f;

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
