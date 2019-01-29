using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternIterator : MonoBehaviour {

    public IPattern pattern;
    public float delay;

    private void OnEnable()
    {
        StartCoroutine(AttackIterator());
    }

    IEnumerator AttackIterator()
    {
        yield return new WaitUntil(() => PathFinder.Ins.isMapValid);
        while (isActiveAndEnabled)
        {
            pattern.TriggerPattern();

            yield return new WaitForSeconds(pattern.Time + delay);
        }
    }

}
