using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossBrain : MonoBehaviour {

    public float delayBetweenPatterns;
    public BossPattern[] patterns;
    public Transform pos;
    public Vector3Int[] tilesToShut;
    public GameObject image;

    int currentPatternIndex = -1;
    Movement_Marching movement;

    const float LOCKON_TIME = 3f;

    private void Awake()
    {
        movement = GetComponent<Movement_Marching>();
    }

    private void OnEnable()
    {
        StartCoroutine(ShowUp());
    }

    IEnumerator ShowUp()
    {
        //yield return new WaitForSeconds(2);
        Transform player = GameManager.Instance.player;
        Transform cam = Camera.main.transform;

        player.GetComponent<PlayerMovement>().isAllowedToMove = false;
        GetComponent<Movement_Marching>().isMarching = false;

        CameraController.Instance.target = transform;
        yield return new WaitForSeconds(LOCKON_TIME/2);

        //image.SetActive(true); // 등장 일러스트 enable
        PathController.Instance.Shut(tilesToShut);
        yield return new WaitForSeconds(LOCKON_TIME/2);
        //image.SetActive(false); // 등장 일러스트 disable

        CameraController.Instance.target = GameManager.Instance.player;
        yield return new WaitForSeconds(LOCKON_TIME/3);

        player.GetComponent<PlayerMovement>().isAllowedToMove = true;
        GetComponent<Movement_Marching>().isMarching = true;

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
                    movement.isMarching = true;
                    movement.target = GameManager.Instance.player;

                    yield return new WaitForSeconds(pattern.behaviour.Time + delayBetweenPatterns);
                    break;
                case MovementType.Still:
                    movement.isMarching = false;
                    movement.target = GameManager.Instance.player;

                    yield return new WaitForSeconds(pattern.behaviour.Time + delayBetweenPatterns);
                    break;
                case MovementType.GoTo:
                    movement.isMarching = true;
                    movement.target = pos;

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
