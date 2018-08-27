using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Marching : MonoBehaviour
{

    public float speed;
    public float minDstToTarget;

    protected Rigidbody2D rb2d;

    Coroutine tracker;
    ContactFilter2D contactFilter;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        contactFilter.useLayerMask = true;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(PathFinder());
    }

    IEnumerator PathFinder()
    {
        while (true)
        {
            RaycastHit2D wallRay = Physics2D.Raycast(
                transform.position,
                GameManager.Instance.player.position - transform.position,
                Vector2.Distance(transform.position, GameManager.Instance.player.position) + 1,
                GameManager.Instance.layerMask_Wall);

            if (!wallRay)
            {
                OnPathFound(new Vector2[] { GameManager.Instance.player.position }, true);
            }
            else
            {
                PathRequestManager.RequestPath(transform.position, GameManager.Instance.player.position, OnPathFound);
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    protected virtual IEnumerator PathTracker(Vector2[] path)
    {
        int currentIndex = 0;
        float movingCountdown = Vector2.Distance(path[0], transform.position) / speed;
        Vector2 movePerSec = (path[0] - (Vector2)transform.position).normalized * speed;

        while (true)
        {
            Vector2 currentWaypoint = path[currentIndex];

            RaycastHit2D minDstRay = Physics2D.Raycast(rb2d.position, ((Vector2)GameManager.Instance.player.position - rb2d.position).normalized, minDstToTarget, GameManager.Instance.layerMask_Wall_Player);

            if (minDstRay.collider && minDstRay.collider.CompareTag("Player")) // 최소 거리 도달
            {
                rb2d.velocity = Vector2.zero;
                yield break;
            }

            if (movingCountdown <= 0) // waypoint 도달
            {
                rb2d.position = currentWaypoint;

                currentIndex++;
                if (currentIndex >= path.Length)
                {
                    yield break;
                }

                currentWaypoint = path[currentIndex];
                movePerSec = (currentWaypoint - rb2d.position).normalized * speed;
                movingCountdown = Vector2.Distance(currentWaypoint, rb2d.position) / speed;
            }

            rb2d.MovePosition(rb2d.position + movePerSec * Time.deltaTime);
            movingCountdown -= Time.deltaTime;

            yield return null;
        }
    }

    void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        if (tracker != null)
            StopCoroutine(tracker);

        if (pathSuccessful && gameObject.activeSelf)
        {
            tracker = StartCoroutine(PathTracker(newPath));
        }
    }
}
