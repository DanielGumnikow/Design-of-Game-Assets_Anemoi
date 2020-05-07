using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Soundcontrollerscript : MonoBehaviour
{
    public AudioSource[] audioSource;
    private AudioSource currentSource;
    private AudioMixer MainMix;

    public static Soundcontrollerscript soundInstance;

    private void Awake()
    {
        soundInstance = this;
        //MainMix = Resources.Load("MainMix") as AudioMixer;
    }

    public void stopMainMix()
    {
        //MainMix.
    }



    public void playAudioSource(int AudioSourceIndex)
    {
        if (!audioSource[AudioSourceIndex].isPlaying){
        audioSource[AudioSourceIndex].Play();
        currentSource = audioSource[AudioSourceIndex];
        }
    }

    public void pauseAudioSource()
    {
        currentSource.Pause();
    }

}
