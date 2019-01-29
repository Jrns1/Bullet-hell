using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneUI : MonoBehaviour
{
    public Animator[] animators;
    public float delay;

    public void ShowUpSaveFiles()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            StartCoroutine(Show(i));
        }
    }

    IEnumerator Show(int index)
    {
        yield return new WaitForSeconds(delay * index);
        animators[index].Play("Show");
    }

}
