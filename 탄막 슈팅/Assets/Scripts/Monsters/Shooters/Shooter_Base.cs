using System.Collections;
using UnityEngine;

public class Shooter_Base<T> : IPattern
    where T : IBulletData
{
    public string bulletName;
    public T[] pattern;

    public override float Time
    {
        get
        {
            float t = 0;
            for (int i = 0; i < pattern.Length; i++)
            {
                t += pattern[i].Delay;
            }

            return t;
        }
    }

    protected GameObject Bullet
    {
        get
        {
            return ObjectPool.Ins.PopFromPool(bulletName, transform.position);
        }
    }

    public override void TriggerPattern()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        for (int i = 0; i < pattern.Length && isActive; i++)
        {
            Shoot(pattern[i]);

            if (pattern[i].Delay > 0)
                yield return new WaitForSeconds(pattern[i].Delay);
        }

    }

    protected virtual void Shoot(T data) { }
}