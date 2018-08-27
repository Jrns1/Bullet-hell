using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    protected static T _instance;
    
    
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    return null;
                    //Debug.LogError("There's no active " + typeof(T) + " in this scene");
                }
            }

            return _instance;
        }
    }

    new protected void DontDestroyOnLoad(Object obj)
    {
        if (_instance != null)
        {
            Destroy(obj);
            return;
        }
        Object.DontDestroyOnLoad(obj);
    }
}
