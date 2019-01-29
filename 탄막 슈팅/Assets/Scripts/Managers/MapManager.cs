using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager> {

    public BossSpawnData boss;
    public GameObject rainCloud;
    public SceneEntryPosData[] sceneEntryDatas;
    public string[] scenes;
    public Region[] regions;

    [HideInInspector] public int currentRegionNum = 0;
    [HideInInspector] public int previousRegionNum = -1;

    List<GameObject> spawnedMonsters = new List<GameObject>();
    int previousMonsterCnt = 0;


    ObjectPool objectPool;

    private void Awake()
    {
        objectPool = ObjectPool.Ins;
        if (rainCloud)
        {
            Instantiate(rainCloud).transform.SetParent(GameManager.Ins.player);
        }
    }

    private void Start()
    {
        Fader.Ins.Fade(GameManager.Ins.panel, GameManager.FADING_TIME, 1, 0);
    }

    public void InitScene()
    {
        for (int i = 0; i < sceneEntryDatas.Length; i++)
        {
            if (sceneEntryDatas[i].lastPortalName.Equals(GameManager.Ins.sceneEntryPortalName))
            {
                GameManager.Ins.player.position = sceneEntryDatas[i].entryPos;

                currentRegionNum = sceneEntryDatas[i].regionNum;
                previousRegionNum = currentRegionNum;
                break;
            }
        }

        CameraController.Ins.SetLimit(regions[currentRegionNum].upperRight, regions[currentRegionNum].lowerLeft);
        PathFinder.Ins.SetMap();
        SpawnMonsters();

        regions[currentRegionNum].isDiscovered = true;
        previousMonsterCnt = spawnedMonsters.Count;

        //Debug.LogError("Wrong portal name! : " + GameManager.Ins.sceneEntryPortalName);
    }

    public void EnterRegion(int regionNumber)
    {
        if (regionNumber < 0) // 다른 씬 로드
        {
            int index = -regionNumber - 1;
            StartCoroutine(GameManager.Ins.EnterScene(scenes[index]));
            return;
        }

        currentRegionNum = regionNumber;
        regions[currentRegionNum].isDiscovered = true;
        SpawnMonsters();

        PathFinder.Ins.isMapValid = false;
        CameraController.Ins.SetLimit(regions[currentRegionNum].upperRight, regions[currentRegionNum].lowerLeft);
    }

    public void EndEnteringRegion()
    {
        DisposeMonsters();
        PathFinder.Ins.SetMap();

        previousMonsterCnt = spawnedMonsters.Count;
        previousRegionNum = currentRegionNum;
    }

    void DisposeMonsters()
    {
        if (boss.gameObject && boss.regionNumber == previousRegionNum)
        {
            boss.gameObject.SetActive(false);
        }

        if (spawnedMonsters != null)
        {
            for (int i = 0; i < previousMonsterCnt; i++)
            {
                if (!spawnedMonsters[i].activeSelf)
                    regions[previousRegionNum].monsters[i].dead = true;
                objectPool.PushToPool(spawnedMonsters[i]);
            }   
            spawnedMonsters.RemoveRange(0, previousMonsterCnt);
        }
    }

    void SpawnMonsters()
    {
        if (boss.gameObject &&
            boss.regionNumber == currentRegionNum &&
            !DataManager.Ins.IsDead(boss.gameObject.name))

            boss.gameObject.SetActive(true);
        
        MonsterSpawnData[] monsters = regions[currentRegionNum].monsters;
        for (int i = 0; i < monsters.Length; i++)
        {
            if (!monsters[i].dead)
                spawnedMonsters.Add(Spawn(monsters[i]));
        }
    }

    GameObject Spawn(MonsterSpawnData data)
    {
        return objectPool.PopFromPool(data.name.ToString(), data.spawnPoint);
    }
}
