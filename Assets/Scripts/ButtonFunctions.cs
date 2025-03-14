using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class ButtonFunctions : MonoBehaviour
{
    public Camera mainCamera;
    private GameObject goTarget;
    private AudioManager audioManager;

    void Awake() {
        audioManager = (AudioManager) GameObject.Find("AudioManager").GetComponent(typeof(AudioManager));
    }
    void Start() {
        //mainCamera = Camera.main;
    }

    public void resumeTime()
    {
        if (mainCamera == null) {
            Debug.Log("buttonFunction has no mainCam");
        }
        Time.timeScale = 1.0f;
        if (mainCamera.GetComponent<CameraPan>() != null) {
            mainCamera.GetComponent<CameraPan>().canPan = true;
        }
        
    }

    public void stopTime()
    {
        if (mainCamera == null) {
            Debug.Log("buttonFunction has no mainCam");
        }
        Time.timeScale = 0.0f;
        if (mainCamera.GetComponent<CameraPan>() != null) {
            mainCamera.GetComponent<CameraPan>().canPan = false;
        }
        
    }
    
    public void toggleTime()
    {
        //Time.timeScale == 1.0f? Time.timeScale = 0.0f : Time.timeScale = 1.0f;
        if (Time.timeScale == 1.0f) { //freeze time, stop camera panning
            stopTime();
        } else { //resume normal
            resumeTime();
        }
    }

    public void assignGoTarget(GameObject target) {
        goTarget = target;
    }

    public void sendEventToFSM(string _event) {
        if (goTarget != null) {
            goTarget.GetComponent<PlayMakerFSM>().SendEvent(_event);
        } else {
            Debug.Log("assignGoTarget before sending event");
        }
    }

    /////////////////CARRYING OVER AUDIOMANAGER FUNCTIONS//////////
    
    public void playMusic(string musicName) {
        if (audioManager != null) {
            audioManager.playMusic(musicName);
        } else {
            Debug.Log("cant find audio manager!");
        }
    }

    public void playSoundEffect(string sfxName) {
        if (audioManager != null) {
            audioManager.playSoundEffect(sfxName);
        } else {
            Debug.Log("cant find audio manager!");
        }
    }

    public void updateMusicVol(float _volume)
    {
        if (audioManager != null) {
            audioManager.updateMusicVol(_volume);
        } else {
            Debug.Log("cant find audio manager!");
        }
    }

    public void updateSoundEffectsVol(float _volume) {
        if (audioManager != null) {
            audioManager.updateSoundEffectsVol(_volume);
        } else {
            Debug.Log("cant find audio manager!");
        }
    }

    public float getMusicVol() {
        if (audioManager != null) {
            return audioManager.getMusicVol();
        } else {
            Debug.Log("cant find audio manager!");
            return 1.0f;
        }
    }

    public float getSoundEffectVol() {
        if (audioManager != null) {
            return audioManager.getSoundEffectVol();
        } else {
            Debug.Log("cant find audio manager!");
            return 1.0f;
        }
    }

    public void saveSettings() {
        if (audioManager != null) {
            audioManager.saveSettings();
        } else {
            Debug.Log("cant find audio manager!");
        }
    }
}
