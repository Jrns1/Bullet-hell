using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : Singleton<GameManager> {

    public Transform player;
    public string sceneEntryPortalName;

    [HideInInspector] public int layerMask_Wall_Player;
    [HideInInspector] public int layerMask_Wall;

    bool pause = false;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        layerMask_Wall_Player = (1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Player"));
        layerMask_Wall = 1 << LayerMask.NameToLayer("Wall");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            player.GetComponent<PlayerMovement>().isMoving = !pause;

            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public static bool IsNear(Vector2 a, Vector2 b, float distance)
    {
        Vector2 diff = a - b;
        return diff.x * diff.x + diff.y * diff.y <= distance * distance;
    }

    public static IEnumerator Delay(Action action)
    {
        yield return null;
        action();
    }

    public static Vector2 RotateVector(Vector2 vector, float degree)
    {
        float desiredDeg = Mathf.Atan2(vector.y, vector.x) + degree * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(desiredDeg), Mathf.Sin(desiredDeg)).normalized;
    }
}
