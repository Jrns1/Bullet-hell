using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int maxHealth;
    //public Text test; 

    public event Action PlayerDeathEvent;

    int health;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        PlayerDeathEvent += DeathLog;
        ResetHealth();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health -= 1;
            //UpdateUI();
            if (health <= 0)
            {
                PlayerDeathEvent();
            }
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    void UpdateUI()
    {
        //test.text = health.ToString();
    }

    void DeathLog()
    {
        //Debug.Log("dead");
    }
}
