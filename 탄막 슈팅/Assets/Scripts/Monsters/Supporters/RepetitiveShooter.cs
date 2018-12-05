using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepetitiveShooter : IPattern {

    public override float Time
    {
        get
        {
            return shooter.Time * repeat;
        }
    }

    public int repeat;

    Shooter_Base shooter;

    private void Awake()
    {
        shooter = GetComponent<Shooter_Base>();
    }

    public override void TriggerPattern()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        for (int i = 0; i < repeat; i++)
        {
            yield return new WaitUntil(() => (shooter.isShooting));
            yield return StartCoroutine(shooter.Attack());
        }
    }

}
