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
        objectPool = ObjectPool.Instance;
        if (rainCloud)
        {
            Instantiate(rainCloud).transform.SetParent(GameManager.Instance.player);
        }
    }

    private void Start()
    {
        Fader.Instance.Fade(GameManager.Instance.panel, GameManager.FADING_TIME, 1, 0);
    }

    public void InitScene()
    {
        for (int i = 0; i < sceneEntryDatas.Length; i++)
        {
            if (sceneEntryDatas[i].lastPortalName.Equals(GameManager.Instance.sceneEntryPortalName))
            {
                GameManager.Instance.player.position = sceneEntryDatas[i].entryPos;

                currentRegionNum = sceneEntryDatas[i].regionNum;
                previousRegionNum = currentRegionNum;
                break;
            }
        }

        CameraController.Instance.SetLimit(regions[currentRegionNum].upperRight, regions[currentRegionNum].lowerLeft);
        PathFinder.Instance.SetMap();
        SpawnMonsters();

        regions[currentRegionNum].isDiscovered = true;
        previousMonsterCnt = spawnedMonsters.Count;

        //Debug.LogError("Wrong portal name! : " + GameManager.Instance.sceneEntryPortalName);
    }

    public void EnterRegion(int regionNumber)
    {
        if (regionNumber < 0) // 다른 씬 로드
        {
            int index = -regionNumber - 1;
            StartCoroutine(GameManager.Instance.EnterScene(scenes[index]));
            return;
        }

        currentRegionNum = regionNumber;

        regions[currentRegionNum].isDiscovered = true;
        CameraController.Instance.SetLimit(regions[currentRegionNum].upperRight, regions[currentRegionNum].lowerLeft);
        SpawnMonsters();
    }

    public void EndEnteringRegion()
    {
        DisposeMonsters();
        PathFinder.Instance.SetMap();

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
        bool isAlive = true;

        if (boss.gameObject &&
            boss.regionNumber == currentRegionNum &&
            !TestData.Load<bool>(TestData.DeathPath(boss.gameObject.name), out isAlive))

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
