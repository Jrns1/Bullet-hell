using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    public Action SetValue;
    new public MaskableGraphic renderer;
    Coroutine UIFading;

    public void UpdateUI()
    {
        SetValue();
        if (UIFading != null)
            StopCoroutine(UIFading);
        renderer.color = Color.white;
        UIFading = GameManager.Ins.Delay(() => UIFading = Fader.Ins.Fade(renderer, 1, 1, 0), 1);
    }
}
