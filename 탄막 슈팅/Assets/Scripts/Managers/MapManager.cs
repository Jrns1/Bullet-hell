using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : Singleton<MapManager> {

    public Image panel;

    public SceneEntryPosData[] sceneEntryDatas;
    public string[] scenes;
    public Region[] regions;
    public BossSpawnData boss;

    [HideInInspector] public int currentRegionNum = 0;

    List<GameObject> spawnedMonsters = new List<GameObject>();
    int previousMonsterCnt = 0;
    int previousRegionNum = -1;

    const float fadingTime = .5f;

    ObjectPool objectPool;


    private void Awake()
    {
        objectPool = ObjectPool.Instance;
    }

    private void Start()
    {
        Instance.Fade(panel, fadingTime, 1, 0);
    }

    public void InitScene(Transform player)
    {
        for (int i = 0; i < sceneEntryDatas.Length; i++)
        {
            if (sceneEntryDatas[i].lastPortalName.Equals(GameManager.Instance.sceneEntryPortalName))
            {
                player.position = sceneEntryDatas[i].entryPos;

                currentRegionNum = sceneEntryDatas[i].regionNum;
                previousRegionNum = currentRegionNum;

                CameraController.Instance.SetLimit(regions[currentRegionNum].upperRight, regions[currentRegionNum].lowerLeft);
                PathFinder.Instance.SetMap();
                SpawnMonsters();

                regions[currentRegionNum].isDiscovered = true;
                previousMonsterCnt = spawnedMonsters.Count;

                return;
            }
        }

        Debug.LogError("Wrong portal name! : " + GameManager.Instance.sceneEntryPortalName);
    }

    public void EnterRegion(int regionNumber)
    {
        if (regionNumber < 0) // 다른 씬 로드
        {
            int index = -regionNumber - 1;
            StartCoroutine(EnterScene(scenes[index]));
            return;
        }

        currentRegionNum = regionNumber;

        regions[currentRegionNum].isDiscovered = true;
        CameraController.Instance.SetLimit(regions[currentRegionNum].upperRight, regions[currentRegionNum].lowerLeft);
        SpawnMonsters();
    }

    IEnumerator EnterScene(string sceneName)
    {
        yield return Instance.Fade(panel, fadingTime);
        SceneManager.LoadScene(sceneName);
    }

    public void EndMoving()
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
            !Data.Load<bool>(Data.DeathPath(boss.gameObject.name), out isAlive))

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
