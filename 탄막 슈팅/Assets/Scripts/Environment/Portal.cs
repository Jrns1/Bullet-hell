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
            StartCoroutine(MoveTo(GameManager.Instance.player, collision));
        }
    }

    IEnumerator MoveTo(Transform target, Collider2D foot)
    {
        MapManager.Instance.EnterRegion(regionNumber);
        GameManager.Instance.sceneEntryPortalName = name;

        PathFinder.Instance.isMapValid = false;
        PlayerMovement movement = target.GetComponent<PlayerMovement>();
        float speed = movement.runningSpeed;

        movement.isAllowedToMove = false;
        foot.enabled = false;

        while ((Vector2)target.position != goal)
        {
            target.position = Vector2.MoveTowards(target.position, goal, speed * Time.deltaTime);
            yield return null;
        }

        movement.isAllowedToMove = true;
        foot.enabled = true;

        MapManager.Instance.EndEnteringRegion();
    }

}
