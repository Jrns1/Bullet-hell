using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet_Bouncy : MonoBehaviour {

    Rigidbody2D rb2d;
    public int maxBounce;

    int bounceCnt;
    float speed;
    Vector2 direction;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        bounceCnt = 0;
        StartCoroutine(GameManager.Delay(SetDirection));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            if (bounceCnt >= maxBounce)
            {
                ObjectPool.Instance.PushToPool(gameObject);
                return;
            }
            
            rb2d.velocity = Vector2.Reflect(direction, collision.contacts[0].normal).normalized * speed;
            bounceCnt++;
            SetDirection();
        }
    }

    void SetDirection()
    {
        direction = rb2d.velocity;
        speed = rb2d.velocity.magnitude;
    }
}
