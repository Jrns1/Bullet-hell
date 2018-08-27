using System.Collections;
using UnityEngine;

public class Shooter_Base : MonoBehaviour
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
    

    public virtual IEnumerator Attack()
    {

        for (int i = 0; i < Length && isShooting; i++)
        {
            IBulletData data = GetBulletData(i);
            Shoot(data);

            if (data.Delay > 0)
                yield return new WaitForSeconds(data.Delay);
        }

    }

    protected virtual void Shoot(IBulletData bulletData) { }

    protected virtual IBulletData GetBulletData(int index)
    {
        return null;
    }
}