using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour {

    const float INTERACTIVE_DISTANCE = .8f;

	void Update () {
        if (Input.GetKeyDown(KeyCode.E) && GameManager.IsNear(GameManager.Instance.player.position, transform.position, INTERACTIVE_DISTANCE) && GameManager.Instance.isInteractionAllowed)
        {
            GameManager.Instance.isInteractionAllowed = false;
            Interact();
        }
	}

    protected virtual void Interact() { GameManager.Instance.isInteractionAllowed = true; }

}
