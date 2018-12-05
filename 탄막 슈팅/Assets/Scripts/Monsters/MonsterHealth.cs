using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour {

    public float maxHealth;
    float health;

    private const float SQUARED_KNOCKBACK_CLEAR_MAG = .3f;

    bool isKnockbackBeingDone;
    Rigidbody2D rb2d;
    Movement_Marching movement;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement_Marching>();
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player's Bullet"))
        {
            Bullet_Player bullet = collision.GetComponent<Bullet_Player>();
            bullet.OnCollisionWithTarget();
            health -= bullet.damage;
            Knockback(bullet.knockbackForce);

            if (health <= 0)
                Die();
        }
    }

    void Knockback(Vector2 knockback)
    {
        rb2d.velocity = knockback;
        movement.isMarching = false;

        if (!isKnockbackBeingDone)
        {
            StartCoroutine(KnockbackRemover());
        }
    }

    IEnumerator KnockbackRemover()
    {
        isKnockbackBeingDone = true;
        yield return new WaitUntil(() => (IsKnockbackCleared(rb2d.velocity)));
        isKnockbackBeingDone = false;

        movement.isMarching = true;
        rb2d.velocity = Vector2.zero;
    }


    bool IsKnockbackCleared(Vector2 velocity)
    {
        return velocity.x * velocity.x + velocity.y * velocity.y < SQUARED_KNOCKBACK_CLEAR_MAG;
    }


    protected virtual void Die()
    {
        ObjectPool.Instance.PushToPool(gameObject);
    }
}
