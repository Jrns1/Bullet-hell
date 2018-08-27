using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour {

    const float interactiveDistance = .8f;

	void Update () {
        if (Input.GetKeyDown(KeyCode.E) && GameManager.IsNear(GameManager.Instance.player.position, transform.position, interactiveDistance))
        {
            Interact();
        }
	}

    protected virtual void Interact() { Debug.Log("INTERACT"); }

}
