using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterIterator : MonoBehaviour {

    Shooter_Base shooter;

    private void Awake()
    {
        shooter = GetComponent<Shooter_Base>();
    }

    private void OnEnable()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitUntil(() => (PathFinder.Instance.isMapValid));
        while (isActiveAndEnabled)
        {
            if (shooter.isShooting)
            {
                shooter.TriggerPattern();
                yield return StartCoroutine(shooter.Attack());
            }
            else yield return null;
        }
    }

}
