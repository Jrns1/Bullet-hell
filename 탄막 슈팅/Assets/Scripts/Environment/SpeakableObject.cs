using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakableObject : InteractiveObject {

    protected override void Interact()
    {
        StartCoroutine(DialogueManager.Instance.StartDialogue());
    }

}
