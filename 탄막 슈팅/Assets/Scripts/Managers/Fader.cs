using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Fader : Singleton<Fader>
{
    public Coroutine Fade(Graphic renderer, float time, float start, float end)
    {
        Color c = renderer.color;
        return StartCoroutine(FadeCo(
            (a) => renderer.color = new Color(c.r, c.g, c.b, a),
            time, start, end
            ));
    }

    public Coroutine Fade(SpriteRenderer renderer, float time, float start, float end)
    {
        Color c = renderer.color;
        return StartCoroutine(FadeCo(
            (a) => renderer.color = new Color(c.r, c.g, c.b, a),
            time, start, end
            ));
    }

    public IEnumerator FadeCo(Action<float> Set, float time, float start, float end)
    {
        float t = 0;
        float totalChange = time / (end - start);

        while (t < time)
        {
            Set(start + t / totalChange);
            t += Time.deltaTime;
            yield return null;
        }

        Set(end);
    }
}