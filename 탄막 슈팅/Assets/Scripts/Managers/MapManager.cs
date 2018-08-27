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

    [HideInInspector] public int currentRegionNum = 0;

    List<GameObject> spawnedMonsters = new List<GameObject>();

    const float fadingTime = .5f;

    ObjectPool objectPool;


    private void Awake()
    {
        objectPool = ObjectPool.Instance;
    }

    private void Start()
    {
        Instance.Fade(panel, fadingTime, 1, 0);
        PathFinder.Instance.SetMap();
    }

    public Vector2 GetScenePosAndSomeFunc(string portalName)
    {
        for (int i = 0; i < sceneEntryDatas.Length; i++)
        {
            if (sceneEntryDatas[i].lastPortalName.Equals(portalName))
            {
                currentRegionNum = sceneEntryDatas[i].regionNum;
                CameraController.Instance.SetLimit(regions[currentRegionNum].upperRight, regions[currentRegionNum].lowerLeft);
                regions[currentRegionNum].isDiscovered = true;
                SpawnMonsters();

                return sceneEntryDatas[i].entryPos;
            }
        }

        Debug.LogError("Wrong portal name!");
        return Vector2.zero;
    }

    public void EnterRegion(int regionNumber)
    {
        if (regionNumber < 0) // 다른 씬 로드
        {
            int index = -regionNumber - 1;
            StartCoroutine(EnterScene(scenes[index]));
            return;
        }

        DisposeMonsters();
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

    void DisposeMonsters()
    {
        if (spawnedMonsters != null)
        {
            for (int i = 0; i < spawnedMonsters.Count; i++)
            {
                if (spawnedMonsters[i].activeSelf)
                    objectPool.PushToPool(spawnedMonsters[i]);
                else
                    regions[currentRegionNum].monsters[i].dead = true;
            }
            spawnedMonsters.Clear();
        }
    }

    void SpawnMonsters()
    {
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
