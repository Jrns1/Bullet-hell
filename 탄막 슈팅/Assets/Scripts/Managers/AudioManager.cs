using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager> {

    [SerializeField] Music[] musics;
    [SerializeField] SoundEffect[] soundEffects;

    public const float soundEffectDelay = .1f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < musics.Length; i++)
        {
            musics[i].SetSource(gameObject.AddComponent<AudioSource>());
        }
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].Init();
        }
    }

    public void PlaySoundEffect(string _name)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].name.Equals(_name))
            {
                soundEffects[i].Play();
                return;
            }
        }

        // no sound found
        Debug.LogWarning("No sound named " + _name + " found");
    }

    public void PlayMusic(string _name, float time=0f)
    {
        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].name.Equals(_name))
            {
                musics[i].Play();
                if (time > 0)
                    StartCoroutine(FadeInCo(musics[i], time));

                return;
            }
        }

        // no sound found
        Debug.LogWarning("No sound named " + _name + " found");
    }

    public void StopMusic(string _name, float time=0f)
    {
        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].name.Equals(_name))
            {
                if (time > 0)
                    StartCoroutine(FadeOutCo(musics[i], time));
                else
                    musics[i].Stop();
                
                return;
            }
        }

        // no sound found
        Debug.LogWarning("No sound named " + _name + " found");
    }
    
    private IEnumerator FadeInCo(Music sound, float time)
    {
        float t = 0;
        float originalVolume = sound.volume;

        while (t < time)
        {
            sound.CurrentVolume = originalVolume * t / time;
            t += Time.deltaTime;
            yield return null;
        }

        sound.CurrentVolume = originalVolume;
    }

    private IEnumerator FadeOutCo(Music sound, float time)
    {
        float t = time;
        float originalVolume = sound.volume;

        while (t > 0)
        {
            sound.CurrentVolume = originalVolume * t / time;
            t -= Time.deltaTime;
            yield return null;
        }

        sound.Stop();
    }
}

