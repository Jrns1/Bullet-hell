using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using System;

public class BossBrain : MonoBehaviour {

    public float delayBetweenPatterns;
    public BossPattern[] patterns;
    public Transform pos;

    int currentPatternIndex = -1;
    Movement_Marching foot;

    private void Awake()
    {
        foot = GetComponent<Movement_Marching>();
    }

    private void OnEnable()
    {
        StartCoroutine(PatternIterator());
    }

    public IEnumerator PatternIterator()
    {
        while (enabled)
        {
            BossPattern pattern = patterns[GetRandomPattern()];

            if (pattern.behaviour)
                pattern.behaviour.TriggerPattern();

            switch (pattern.movement)
            {
                case MovementType.March:
                    foot.isMarching = true;
                    foot.target = GameManager.Instance.player;

                    yield return new WaitForSeconds(pattern.behaviour.Time + delayBetweenPatterns);
                    break;
                case MovementType.Still:
                    foot.isMarching = false;
                    foot.target = GameManager.Instance.player;

                    yield return new WaitForSeconds(pattern.behaviour.Time + delayBetweenPatterns);
                    break;
                case MovementType.GoTo:
                    foot.isMarching = true;
                    foot.target = pos;

                    if (pattern.behaviour == null)
                    {
                        yield return new WaitUntil(() => GameManager.IsNear(transform.position, pos.position, 0.05f));
                    }
                    else
                    {
                        float goal = Time.time + pattern.behaviour.Time + delayBetweenPatterns;

                        yield return new WaitUntil(() => (
                        GameManager.IsNear(transform.position, pos.position, 0.05f) && Time.time > goal)); // Time.time이 timescale에 영향을 받더라 -> esc 고려 x
                    }
                    break;
            }
        }
    }

    int GetRandomPattern()
    {
        int index = Random.Range(0, patterns.Length - 1);
        if (index == currentPatternIndex)
        {
            index = index == patterns.Length + 1 ? 0 : index + 1;
        }

        currentPatternIndex = index;
        return index;
    }


}
