using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPattern : MonoBehaviour
{
    public abstract float Time
    {
        get;
    }

    public bool isActive = true;

    public abstract void TriggerPattern();
}

public enum MovementType
{
    March,
    Still,
    GoTo,
}

[System.Serializable]
public struct BossPattern
{
    public MovementType movement;
    public IPattern behaviour;
}