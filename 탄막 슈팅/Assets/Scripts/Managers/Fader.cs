using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Fader : Singleton<Fader>
{
    delegate void ColorFunc(Color color);
    
    public Coroutine Fade(SpriteRenderer renderer, float time, float startingAlpha = 0, float endingAlpha = 1)
    {
        return StartCoroutine(FadeCo(time,
            startingAlpha, 
            endingAlpha,
            renderer.color,
            (color) => renderer.color = color));
    }

    public Coroutine Fade(Image renderer, float time, float startingAlpha = 0, float endingAlpha = 1)
    {
        return StartCoroutine(FadeCo(
            time, 
            startingAlpha, 
            endingAlpha,
            renderer.color,
            (color) => renderer.color = color));
    }

    IEnumerator FadeCo(float time, float startingAlpha, float endingAlpha, Color color, ColorFunc SetColor)
    {
        color.a = startingAlpha;
        float t = 0;
        float changingAlpha = endingAlpha - startingAlpha;

        while (t < time)
        {
            SetColor(color);
            color.a = startingAlpha + t / time * changingAlpha;
            t += Time.deltaTime;
            yield return null;
        }

        color.a = endingAlpha;
        SetColor(color);
    }
}