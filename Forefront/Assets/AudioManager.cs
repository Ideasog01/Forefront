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
        sound.AudioSourceRef.gameObject.SetActive(false);
    }

    private void SpawnSound(Sound sound)
    {
        AudioSource source = null;

        foreach(AudioSource audio in _audioSourceList)
        {
            if(!audio.gameObject.activeSelf)
            {
                audio.gameObject.SetActive(true);
                source = audio;
                break;
            }
        }

        if(source == null)
        {
            source = Instantiate(audioSourcePrefab.GetComponent<AudioSource>(), sound.SoundTransform.position, Quaternion.identity);
            sound.AudioSourceRef = source;
        }

        source.clip = sound.AudioClipRef;
        source.volume = sound.Volume;
        source.pitch = sound.Pitch;

        source.PlayDelayed(sound.DelayTime);
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
    private int volume;

    [SerializeField]
    private int pitch;

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

    public int Volume
    {
        get { return volume; }
    }

    public int Pitch
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
