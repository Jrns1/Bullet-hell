using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Base : MonoBehaviour {

    public float disposeSec = 30f;

    protected Rigidbody2D rb2d;

    Coroutine disposing;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        disposing = StartCoroutine(Dispose());
        GameManager.Ins.Delay(SetRotation);
    }

    void SetRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionWithWall(collision);
    }

    public virtual void OnCollisionWithTarget()
    {
        Kill();
    }

    protected virtual void OnCollisionWithWall(Collision2D collision)
    {
        Kill();
    }

    protected void Kill()
    {
        ObjectPool.Ins.PushToPool(gameObject);
        StopCoroutine(disposing);
    }

    IEnumerator Dispose()
    {
        yield return new WaitForSeconds(disposeSec);
        Kill();
    }
}
