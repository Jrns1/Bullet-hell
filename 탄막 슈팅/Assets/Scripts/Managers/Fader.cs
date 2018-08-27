using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public static class Fader
{
    public static Coroutine Fade(this MonoBehaviour monoBehaviour, SpriteRenderer renderer, float time, float startingAlpha = 0, float endingAlpha = 1)
    {
        return monoBehaviour.StartCoroutine(FadeCo(time, startingAlpha, endingAlpha,
            renderer.color,
            (color) => renderer.color = color));
    }

    public static Coroutine Fade(this MonoBehaviour monoBehaviour, Image renderer, float time, float startingAlpha = 0, float endingAlpha = 1)
    {
        return monoBehaviour.StartCoroutine(FadeCo(time, startingAlpha, endingAlpha,
            renderer.color,
            (color) => renderer.color = color));
    }

    private static IEnumerator FadeCo(float time, float startingAlpha, float endingAlpha, Color color, Action<Color> SetColor)
    {
        float r = color.r;
        float g = color.g;
        float b = color.b;
        float a = startingAlpha;
        float t = 0;
        float changingAlpha = endingAlpha - startingAlpha;

        while (t < time)
        {
            SetColor(new Color(r, g, b, a));
            a = startingAlpha + t / time * changingAlpha;
            t += Time.deltaTime;
            yield return null;
        }

        SetColor(new Color(r, g, b, endingAlpha));
    }
}