using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int maxHealth;
    public float invicibleTime;

    public event Action PlayerDeathEvent;

    int health;
    bool isDamageable = true;

    private void Awake()
    {
        PlayerDeathEvent += RestartFromSP;
    }

    private void OnEnable()
    {
        ResetHealth();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy's Bullet") && isDamageable)
        {
            health -= 1;
            StartCoroutine(Invincibility());
            collision.GetComponent<Bullet_Base>().OnCollisionWithTarget();

            if (health <= 0)
            {
                GetComponent<Collider2D>().enabled = false;
                PlayerDeathEvent();
            }
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    IEnumerator Invincibility()
    {
        isDamageable = false;
        yield return new WaitForSeconds(invicibleTime);
        isDamageable = true;
    }

    void RestartFromSP()
    {
        SceneManager.sceneLoaded += MoveToSP;
        GameManager.Ins.sceneEntryPortalName = null;
        StartCoroutine(GameManager.Ins.EnterScene(GameManager.Ins.savePoint.sceneName));
    }

    void MoveToSP(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= MoveToSP;

        SavePointData sp = GameManager.Ins.savePoint;
        MapManager mapManager = MapManager.Ins;

        transform.position = sp.position;
        mapManager.currentRegionNum = sp.regionNum;
        mapManager.previousRegionNum = sp.regionNum;

        mapManager.InitScene();

        ResetHealth();
        GetComponent<Collider2D>().enabled = true;
    }
}
