using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool> {

    [System.Serializable]
    public class Pool
    {
        public string name;
        //public string tag;

        public GameObject prefab;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        for (int i = 0; i < pools.Count; i++)
        {
            poolDictionary.Add(pools[i].name, new Queue<GameObject>());
        }
    }

    public GameObject PopFromPool(string name, Vector2 position, bool active = true, Transform parent = null)
    {
        GameObject item;
        if (poolDictionary[name].Count <= 0)
        {
            item = CreateObj(name, position);
        }
        else
        {
            item = poolDictionary[name].Dequeue();
            item.transform.position = position;

        }
        item.SetActive(active);
        item.transform.SetParent(parent);
        return item;
    }

    public void PushToPool(GameObject item, Transform parent = null)
    {
        item.SetActive(false);
        item.transform.SetParent(parent ? parent : transform);
        poolDictionary[item.name].Enqueue(item);
    }

    private GameObject CreateObj(string name, Vector2 position)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].name == name)
            {
                GameObject item = Instantiate(pools[i].prefab, position, Quaternion.identity);
                item.name = pools[i].name;
                //item.tag = pools[i].tag;
                return item;
            }
        }

        return null;
    }
}
