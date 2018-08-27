using UnityEngine;
using System.Collections.Generic;
using System.Collections;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;

}

[System.Serializable]
public class Music : Sound
{
    public bool loop = false;

    protected AudioSource source;

    public float CurrentVolume
    {
        get
        {
            return source.volume;
        }
        set
        {
            source.volume = value;
        }
    }

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public virtual void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

[System.Serializable]
public class SoundEffect : Sound
{
    [Range(0.5f, 1.5f)]
    public float pitch = 1;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    private GameObject sourceObject;
    private Queue<AudioSource> sources;
    private float allowedTime;

    public void Init()
    {
        sources = new Queue<AudioSource>();
        sourceObject = new GameObject(name);
        sourceObject.transform.SetParent(AudioManager.Instance.transform);
    }

    public void Play()
    {
        if (allowedTime > Time.time)
        {
            return;
        }

        AudioSource source;
        if (sources.Count <= 0)
        {
            source = sourceObject.AddComponent<AudioSource>();
            source.clip = clip;
        }
        else
        {
            source = sources.Dequeue();
        }
        AudioManager.Instance.StartCoroutine(Enqueue(source));

        allowedTime = Time.time + AudioManager.soundEffectDelay;
        source.volume = volume * (1 + Random.Range(-randomVolume / 2, randomVolume / 2));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2, randomPitch / 2));

        source.Play();
    }

    IEnumerator Enqueue(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        sources.Enqueue(source);
    }
}