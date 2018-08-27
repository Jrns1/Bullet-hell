using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTrigger : MonoBehaviour {

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
        while (shooter.isShooting)
        {
            yield return StartCoroutine(shooter.Attack());
        }
    }

}
