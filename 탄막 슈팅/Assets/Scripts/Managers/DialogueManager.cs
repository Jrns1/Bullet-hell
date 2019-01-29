using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager> {

    public Line[] lines;
    public Dictionary<string, Transform> speakers = new Dictionary<string, Transform>();

    public IEnumerator StartDialogue()
    {
        Transform initialTarget = CameraController.Ins.target;
        GameManager.Ins.isActionInhibited = true;
        CameraController.Ins.enableZoom = false;

        SpeechBubble bubble = null;

        for (int line = 0; line < lines.Length; line++)
        {
            if (lines[line].speaker != "\"")
            {
                if (bubble)
                    StartCoroutine(bubble.Dispose());

                CameraController.Ins.target = speakers[lines[line].speaker];

                bubble = ObjectPool.Ins.PopFromPool(
                    "Speech Bubble",
                    speakers[lines[line].speaker].position + new Vector3(0, 3, 0))
                    .GetComponent<SpeechBubble>();
            }

            yield return StartCoroutine(bubble.Type(lines[line].script));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        StartCoroutine(bubble.Dispose());

        CameraController.Ins.target = initialTarget;
        GameManager.Ins.isActionInhibited = false;
        CameraController.Ins.enableZoom = true;

    }
}
