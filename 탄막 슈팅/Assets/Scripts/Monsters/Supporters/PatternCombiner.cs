using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternCombiner : IPattern {

    public IPattern[] patterns;

    public override float Time
    {
        get
        {
            float t = 0;
            for (int i = 0; i < patterns.Length; i++)
            {
                t += patterns[i].Time;
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
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i].TriggerPattern();
            yield return patterns[i].Time;
        }
    }


}
