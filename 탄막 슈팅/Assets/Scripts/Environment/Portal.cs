using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Direction
{
    upward,
    downward,
    leftward,
    rightward
}

public class Portal : MonoBehaviour {

    public int regionNumber;
    public Vector2 goal;

    const float SPEED = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Ins.isActionInhibited)
        {
            StartCoroutine(MoveTo(GameManager.Ins.player));
        }
    }

    IEnumerator MoveTo(Transform target)
    {
        MapManager.Ins.EnterRegion(regionNumber);
        GameManager.Ins.sceneEntryPortalName = name;


        GameManager.Ins.isActionInhibited = true;

        while ((Vector2)target.position != goal)
        {
            target.position = Vector2.MoveTowards(target.position, goal, SPEED * Time.deltaTime);
            yield return null;
        }

        GameManager.Ins.isActionInhibited = false;

        MapManager.Ins.EndEnteringRegion();
    }

}
