using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    private float musicVol, soundEffectVol;
    private string currMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            loadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        musicSource.Play();
        currMusic = "outdoor";
        Application.runInBackground = true;
    }

    public void playMusic(string musicName) {
        Sound s = Array.Find(musicSounds, sound => sound.name == musicName);
        if (s != null) {
            if (s.name != currMusic) {
                musicSource.clip = s.clip;
                //Debug.Log("music change from: " + currMusic);
                currMusic = s.name;
                //Debug.Log("change to: " + currMusic);
                musicSource.Play();
            }
        } else {
            Debug.Log("music not found: " + musicName);
        }
    }

    public void playSoundEffect(string sfxName) {
        Sound s = Array.Find(sfxSounds, sound => sound.name == sfxName);
        if (s != null) {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        } else {
            Debug.Log("sound effect not found: " + sfxName);
        }
    }

    public void updateMusicVol(float _volume)
    {
        //Debug.Log("volume changed to: " + _volume);
        musicVol = _volume;
        musicSource.volume = musicVol;
        PlayerPrefs.SetFloat("Music", musicVol);
    }

    public void updateSoundEffectsVol(float _volume) {
        soundEffectVol = _volume;
        sfxSource.volume = soundEffectVol;
        PlayerPrefs.SetFloat("SoundEffects", soundEffectVol);
    }

    public float getMusicVol() {
        return musicVol;
    }

    public float getSoundEffectVol() {
        return soundEffectVol;
    }

    private void loadSettings() {
        musicVol = PlayerPrefs.GetFloat("Music", 1.0f);
        soundEffectVol = PlayerPrefs.GetFloat("SoundEffects", 1.0f);
        musicSource.volume = musicVol;
        sfxSource.volume = soundEffectVol;
    }


    public void saveSettings() {
        PlayerPrefs.SetFloat("Music", musicVol);
        PlayerPrefs.SetFloat("SoundEffects", soundEffectVol);
    }
    
}

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;
}