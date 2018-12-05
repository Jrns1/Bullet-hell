using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SavePointData
{
    public Vector2 position;
    public string sceneName;
    public int regionNum;

    public SavePointData(Vector2 _position, string _sceneName, int _regionNum)
    {
        position = _position;
        sceneName = _sceneName;
        regionNum = _regionNum;
    }
}
