using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;
    public Sound[] musicSounds, sfxSounds;
    //public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AudioSourceInit();
    }

    private void Start()
    {
        Instance.PlayMusic("PlaceHolder");
    }

    public void AudioSourceInit()
    {
        CreateAudioSources(musicSounds, "Music Audio Source: ");
        CreateAudioSources(sfxSounds, "SFX Audio Source: ");
    }

    private void CreateAudioSources(Sound[] sounds, string prefix)
    {
        foreach (var sound in sounds)
        {
            GameObject child = new GameObject(prefix + sound.name);
            child.transform.parent = transform;

            var source = child.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.outputAudioMixerGroup = sound.outputGroup;
            source.playOnAwake = false;
            sound.audioSource_m = source;
            source.volume = sound.volume;
        }
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20f);
    }
    public void ToggleMasterMute(bool isMuted)
    {
        audioMixer.SetFloat("MasterVolume", isMuted ? -80f : Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1f)) * 20f);
    }
    public void ToggleMusicMute(bool isMuted)
    {
        audioMixer.SetFloat("MusicVolume", isMuted ? -80f : Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1f)) * 20f);
    }

    public void ToggleSFXMute(bool isMuted)
    {
        audioMixer.SetFloat("SFXVolume", isMuted ? -80f : Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1f)) * 20f);
    }

    public void StopAllSound()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void StopMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            s.audioSource_m.Stop();
        }
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            s.audioSource_m.Play();
            s.audioSource_m.loop = true;
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            s.audioSource_m.PlayOneShot(s.clip);
        }
    }

    public void PlaySFXLoop(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            s.audioSource_m.Play();
            s.audioSource_m.loop = true;
        }
    }

    public AudioMixer getAudioMixer()
    {
        return audioMixer;
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup outputGroup;
    public AudioSource audioSource_m;
    [Range(0.0f, 1.0f)]
    public float volume;
}
