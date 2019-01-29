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

    const float PATH_UPDATE_DELAY = .5f;

    Coroutine tracker;
    Vector2 colliderSize;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (!target)
            target = GameManager.Ins.player;
        colliderSize = GetComponent<BoxCollider2D>().size;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(PathUpdater());
    }

    IEnumerator PathUpdater()
    {
        yield return new WaitUntil(() => PathFinder.Ins.isMapValid);
        while (isActiveAndEnabled)
        {
            RaycastHit2D wallRay = Physics2D.BoxCast(
                transform.position,
                colliderSize,
                0,
                target.position - transform.position,
                Vector2.Distance(transform.position, target.position),
                GameManager.Ins.layerMask_Wall);

            if (!wallRay) // 플레이어와 몬스터 사이 장애물이 없는 경우
            {
                OnPathFound(new Vector2[] { target.position }, true);
            }
            else
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            }

            yield return new WaitForSeconds(PATH_UPDATE_DELAY);
        }
    }

    protected virtual IEnumerator PathTracker(Vector2[] path)
    {
        int currentIndex = 0;

        // pathfinding시스템은 주어진 위치를 반올림하여 (int, int)로 만들어 사용한다.
        // 그렇기에 pathfinding의 시작점은 input과 오차가 발생한다.
        // 그럼 실제 위치에서 첫 waypoint를 향해 가면 벽과 부딪힐 수 있다.
        // 그렇다고 무조건 계산 상의 시작점을 가게 하면 시작점을 자꾸 맴도는 오류가 발생할 수 있다.
        // 이를 막기 위해 첫 waypoint로 바로 갈 수 있는지를 확인해야 한다.
        if (path.Length > 1)
        {
            RaycastHit2D wallRay = Physics2D.BoxCast(
                transform.position,
                colliderSize,
                0,
                path[1] - (Vector2)transform.position,
                Vector2.Distance(transform.position, path[1]),
                GameManager.Ins.layerMask_Wall);

            if (!wallRay)
            {
                currentIndex = 1;
            }
        }

        while (isActiveAndEnabled)
        {
            Vector2 currentWaypoint = path[currentIndex];

            RaycastHit2D wallRay = Physics2D.Raycast(rb2d.position, ((Vector2)target.position - rb2d.position).normalized, minDstToTarget, GameManager.Ins.layerMask_Wall);

            if (!wallRay.collider && (target.position - transform.position).sqrMagnitude < GameManager.Sqr(minDstToTarget) || // 최소 거리 도달
                !isMarching)
            {
                //rb2d.velocity = Vector2.zero;
                yield return null;
                continue;
            }

            if ((rb2d.position - currentWaypoint).sqrMagnitude < .05f) // waypoint 도달
            {
                rb2d.position = currentWaypoint;

                currentIndex++;
                if (currentIndex >= path.Length)
                {
                    yield break;
                }

                currentWaypoint = path[currentIndex];
            }

            rb2d.MovePosition(Vector2.MoveTowards(rb2d.position, currentWaypoint, speed * Time.fixedDeltaTime));

            yield return new WaitForFixedUpdate();
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
