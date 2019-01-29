using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MonsterName
{
    Test,
    Test_Triple
}

[System.Serializable]
public struct MonsterSpawnData
{
    public MonsterName name;
    public Vector2 spawnPoint;
    public bool dead;

    public MonsterSpawnData(Vector2 _point, MonsterName _name)
    {
        name = _name;
        spawnPoint = _point;
        dead = false;
    }
}

[System.Serializable]
public struct BossSpawnData
{
    public GameObject gameObject;
    public int regionNumber;
}

[System.Serializable]
public struct Region
{
    //public string regionName;
    public Vector2 upperRight;
    public Vector2 lowerLeft;
    public bool isDiscovered;
    public MonsterSpawnData[] monsters;

    public Region(/*string _regionName, */Vector2 _upperRight, Vector2 _lowerLeft)
    {
        //regionName = _regionName;
        upperRight = _upperRight;
        lowerLeft = _lowerLeft;
        isDiscovered = false;
        monsters = new MonsterSpawnData[0];
    }
}