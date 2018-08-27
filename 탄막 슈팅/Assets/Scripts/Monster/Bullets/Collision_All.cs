using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_All : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectPool.Instance.PushToPool(gameObject);
    }

}
