using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortByY : MonoBehaviour {

    public float footOffset = -.5f;
    public float floating;

	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + footOffset - floating);
	}
}
