using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Redirector : MonoBehaviour {

    public string sceneName;
    public GameObject[] constantObjects;

    private void Awake()
    {
        foreach (GameObject constObj in constantObjects)
        {
            DontDestroyOnLoad(constObj);
        }

        SceneManager.LoadScene(sceneName);
    }

}
