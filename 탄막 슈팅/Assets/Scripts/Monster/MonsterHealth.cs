using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour {

    public float maxHealth;
    float health;

    private void OnEnable()
    {
        health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<Bullet_Player>().damage;

            if (health <= 0)
                ObjectPool.Instance.PushToPool(gameObject);
        }
    }

}
