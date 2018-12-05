using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Base : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionWithWall(collision);
    }

    public virtual void OnCollisionWithTarget()
    {
        ObjectPool.Instance.PushToPool(gameObject);
    }

    protected virtual void OnCollisionWithWall(Collision2D collision)
    {
        ObjectPool.Instance.PushToPool(gameObject);
    }
}
