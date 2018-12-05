using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public Vector2 force;
    public Vector2 pos;

	void Start () {
        Rigidbody2D a = GetComponent<Rigidbody2D>();
        a.AddForce(force);
        a.MovePosition(pos);
	}
}
