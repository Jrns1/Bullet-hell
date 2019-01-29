using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakableObject : InteractiveObject {

    protected override void OnEnable()
    {
        base.OnEnable();
        DialogueManager.Ins.speakers.Add(name, transform);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (DialogueManager.Ins)
            DialogueManager.Ins.speakers.Remove(name);
    }

    public override void Interact()
    {
        base.Interact();
        // Set dialogue script
        StartCoroutine(WaitAndComplete());
    }

    IEnumerator WaitAndComplete()
    {
        yield return StartCoroutine(DialogueManager.Ins.StartDialogue());
        CompeleteInteraction();
    }

}
