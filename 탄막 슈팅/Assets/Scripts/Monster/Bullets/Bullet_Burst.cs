using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Burst : MonoBehaviour {

    public string bulletName;
    public int cnt;
    public float speed;
    public float burstSec;


    private void OnEnable()
    {
        StartCoroutine(Burst());
    }

    IEnumerator Burst()
    {
        yield return null; // Wait for setting variables
        yield return new WaitForSeconds(burstSec);

        Vector2 direction = (GameManager.Instance.player.position - transform.position).normalized;
        for (int i = 0; i < cnt; i++)
        {
            GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName, transform.position);
            bullet.GetComponent<Rigidbody2D>().velocity = GameManager.RotateVector(direction, 360 / cnt * i) * speed;
        }

        ObjectPool.Instance.PushToPool(gameObject);
    }

}
