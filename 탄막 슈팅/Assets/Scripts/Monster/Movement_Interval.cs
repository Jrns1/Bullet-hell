using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Interval : Movement_Marching {

    public float marchingTime;
    public float intervalTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Interval());
    }

    IEnumerator Interval()
    {
        while (isActiveAndEnabled)
        {
            isMarching = false;
            yield return new WaitForSeconds(marchingTime);
            isMarching = true;
            yield return new WaitForSeconds(intervalTime);
        }
    }
}
