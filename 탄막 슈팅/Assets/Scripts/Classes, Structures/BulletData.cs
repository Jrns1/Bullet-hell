using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBulletData
{
    float Delay { get; }
}

[Serializable]
public struct LinearBulletData : IBulletData
{
    public float delay;
    public float Delay
    {
        get { return delay; }
    }

    public float speed;
}

[Serializable]
public struct AngularBulletData : IBulletData
{
    public float delay;
    public float Delay
    {
        get { return delay; }
    }

    public float speed;
    public float degree;
}

[Serializable]
public struct BurstBulletData : IBulletData
{
    public float delay;
    public float Delay
    {
        get { return delay; }
    }

    public float speed;
    public float burstSecond;
    public int bulletCount;
    public float burstBulletSpeed;
}
