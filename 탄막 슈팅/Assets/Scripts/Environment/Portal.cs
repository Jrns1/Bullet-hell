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
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(MoveTo(GameManager.Instance.player));
            MapManager.Instance.EnterRegion(regionNumber);
            GameManager.Instance.sceneEntryPortalName = name;
        }
    }

    IEnumerator MoveTo(Transform target)
    {
        PathFinder.Instance.isMapValid = false;
        PlayerMovement movement = target.GetComponent<PlayerMovement>();
        Collider2D playerCol = target.GetComponent<Collider2D>();
        float speed = movement.runningSpeed;

        movement.isMoving = false;
        playerCol.enabled = false;

        while ((Vector2)target.position != goal)
        {
            target.position = Vector2.MoveTowards(target.position, goal, speed * Time.deltaTime);
            yield return null;
        }

        movement.isMoving = true;
        playerCol.enabled = true;
        PathFinder.Instance.SetMap();
    }

}
