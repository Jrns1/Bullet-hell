using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepetitiveShooter : IPattern {

    public override float Time
    {
        get
        {
            return pattern.Time * repeat;
        }
    }

    public int repeat;

    IPattern pattern;

    private void Awake()
    {
        pattern = GetComponent<IPattern>();
    }

    public override void TriggerPattern()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        for (int i = 0; i < repeat; i++)
        {
            pattern.TriggerPattern();
            yield return new WaitForSeconds(pattern.Time);
        }
    }

}