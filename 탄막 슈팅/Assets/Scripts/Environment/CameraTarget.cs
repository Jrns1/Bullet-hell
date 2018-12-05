using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {

    private void OnEnable()
    {
        SpeechBubble.Instance.speakers.Add(transform);
    }

    private void OnDisable()
    {
        SpeechBubble.Instance.speakers.Remove(transform);
    }
}
