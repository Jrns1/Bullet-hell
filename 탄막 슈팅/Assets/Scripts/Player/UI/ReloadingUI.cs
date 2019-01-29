using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingUI : MonoBehaviour {

    public float barWidth;

    Transform stick;
    Vector2 initialPos;

    private void Awake()
    {
        stick = transform.GetChild(0);
        initialPos = stick.localPosition;
    }

    public void ShowReloadUI(float reloadingTime)
    {
        gameObject.SetActive(true);
        StartCoroutine(Mover(reloadingTime));
    }

    IEnumerator Mover(float t)
    {
        stick.localPosition = initialPos;
        float movePerSec = barWidth / t;

        while (t >= 0)
        {
            stick.localPosition += Vector3.down * Time.deltaTime * movePerSec;
            t -= Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

}
