using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager> {

    [SerializeField] MusicSource[] musics;
    [SerializeField] SoundEffect[] soundEffects;

    public const float SFX_DELAY = .1f;
    

    private void Awake()
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

    public void PlayMusic(string _name, float fadingTime=0f)
    {
        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].name.Equals(_name))
            {
                musics[i].Play();
                if (fadingTime > 0)
                    StartCoroutine(FadeInCo(musics[i], fadingTime));

                return;
            }
        }

        // no sound found
        Debug.LogWarning("No sound named " + _name + " found");
    }

    public void StopMusic(string _name, float fadingTime=0f)
    {
        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].name.Equals(_name))
            {
                if (fadingTime > 0)
                    StartCoroutine(FadeOutCo(musics[i], fadingTime));
                else
                    musics[i].Stop();
                
                return;
            }
        }

        // no sound found
        Debug.LogWarning("No sound named " + _name + " found");
    }
    
    private IEnumerator FadeInCo(MusicSource sound, float time)
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

    private IEnumerator FadeOutCo(MusicSource sound, float time)
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

