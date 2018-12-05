using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedShooter : IPattern {

    public Shooter_Base[] shooters;

    public override float Time
    {
        get
        {
            float t = 0;
            for (int i = 0; i < shooters.Length; i++)
            {
                t += shooters[i].Time;
            }
            return t;
        }
    }

    public override void TriggerPattern()
    {
        StartCoroutine(Combiner());
    }

    IEnumerator Combiner()
    {
        for (int i = 0; i < shooters.Length; i++)
        {
            yield return StartCoroutine(shooters[i].Attack());
        }
    }


}
