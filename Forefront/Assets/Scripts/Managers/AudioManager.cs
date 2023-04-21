using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    private static List<AudioSource> _audioSourceList = new List<AudioSource>();

    [SerializeField]
    private Transform audioSourcePrefab;

    public void PlaySound(Sound sound)
    {
        if(sound.AudioSourceRef != null)
        {
            sound.AudioSourceRef.PlayDelayed(sound.DelayTime);
        }
        else
        {
            SpawnSound(sound);
        }
    }

    public void StopSound(Sound sound)
    {
        sound.AudioSourceRef.Stop();
    }

    private void SpawnSound(Sound sound)
    {
        AudioSource source = null;

        foreach(AudioSource audio in _audioSourceList)
        {
            if(!audio.isPlaying)
            {
                source = audio;
                break;
            }
        }

        if(source == null && audioSourcePrefab != null && sound.SoundTransform != null)
        {
            source = Instantiate(audioSourcePrefab.GetComponent<AudioSource>(), sound.SoundTransform.position, Quaternion.identity);
            sound.AudioSourceRef = source;
        }

        if(sound.AudioClipRef != null)
        {
            source.clip = sound.AudioClipRef;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;

            source.PlayDelayed(sound.DelayTime);
        }
    }
}

[System.Serializable]
public struct Sound
{
    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float volume;

    [SerializeField]
    private float pitch;

    [SerializeField]
    private float delayTime;

    [SerializeField]
    private Transform soundTransform;

    public AudioClip AudioClipRef
    {
        get { return audioClip; }
    }

    public AudioSource AudioSourceRef
    {
        get { return audioSource; }
        set { audioSource = value; }
    }

    public float Volume
    {
        get { return volume; }
    }

    public float Pitch
    {
        get { return pitch; }
    }

    public float DelayTime
    {
        get { return delayTime; }
    }

    public Transform SoundTransform
    {
        get { return soundTransform; }
    }
}
