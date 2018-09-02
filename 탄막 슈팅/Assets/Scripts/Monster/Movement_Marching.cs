using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Marching : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float minDstToTarget;
    public bool isMarching = true;

    protected Rigidbody2D rb2d;
    protected int layerMask;

    Coroutine tracker;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (!target)
            target = GameManager.Instance.player;
        layerMask = (1 << LayerMask.NameToLayer("Wall") | 1 << target.gameObject.layer);
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(PathUpdater());
    }

    IEnumerator PathUpdater()
    {
        yield return new WaitUntil(() => (PathFinder.Instance.isMapValid));
        while (isActiveAndEnabled)
        {
            RaycastHit2D wallRay = Physics2D.Raycast(
                transform.position,
                target.position - transform.position,
                Vector2.Distance(transform.position, target.position) + 1,
                GameManager.Instance.layerMask_Wall);

            if (!wallRay)
            {
                OnPathFound(new Vector2[] { target.position }, true);
            }
            else
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    protected virtual IEnumerator PathTracker(Vector2[] path)
    {
        int currentIndex = 0;
        float movingCountdown = Vector2.Distance(path[0], transform.position) / speed;
        Vector2 movePerSec = (path[0] - (Vector2)transform.position).normalized * speed;

        while (isActiveAndEnabled)
        {
            Vector2 currentWaypoint = path[currentIndex];

            RaycastHit2D minDstRay = Physics2D.Raycast(rb2d.position, ((Vector2)target.position - rb2d.position).normalized, minDstToTarget, layerMask);

            if (minDstRay.collider && minDstRay.collider.CompareTag("Player") ||
                !isMarching) // 최소 거리 도달
            {
                rb2d.velocity = Vector2.zero;
                yield return null;
                continue;
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
