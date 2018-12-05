using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shooter_Base : IPattern
{
    public string bulletName;
    public bool isShooting = true;

    protected virtual int Length
    {
        get
        {
            return 0;
        }
    }

    public override float Time
    {
        get
        {
            float t = 0;
            for (int i = 0; i < Length; i++)
            {
                t += GetBulletDelay(i);
            }

            return t;
        }
    }

    public override void TriggerPattern()
    {
        StartCoroutine(Attack());
    }

    public virtual IEnumerator Attack()
    {

        for (int i = 0; i < Length && isShooting; i++)
        {
            float delay = GetBulletDelay(i);
            Shoot(i);

            if (delay > 0)
                yield return new WaitForSeconds(delay);
        }

    }

    protected virtual void Shoot(int index) { }
    protected virtual float GetBulletDelay(int index) { return 0; }
}