using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Interval : Movement_Marching {

    public float marchingTime;
    public float intervalTime;

    bool interval;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Interval());
    }

    IEnumerator Interval()
    {
        while (gameObject.activeSelf)
        {
            interval = false;
            yield return new WaitForSeconds(marchingTime);
            interval = true;
            yield return new WaitForSeconds(intervalTime);
        }
    }

    protected override IEnumerator PathTracker(Vector2[] path)
    {
        int currentIndex = 0;
        float movingCountdown = Vector2.Distance(path[0], transform.position) / speed;
        Vector2 movePerSec = (path[0] - (Vector2)transform.position).normalized * speed;

        while (true)
        {
            Vector2 currentWaypoint = path[currentIndex];

            RaycastHit2D minDstRay = Physics2D.Raycast(rb2d.position, ((Vector2)GameManager.Instance.player.position - rb2d.position).normalized, minDstToTarget, GameManager.Instance.layerMask_Wall_Player);

            if (minDstRay.collider && minDstRay.collider.CompareTag("Player") || // 최소 거리 도달
                (interval)) // 휴식
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

}
