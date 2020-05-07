using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundcontrollerscript : MonoBehaviour
{
    public AudioSource[] audioSource;
    private AudioSource currentSource;

    public static Soundcontrollerscript soundInstance;

    private void Awake()
    {
        soundInstance = this;
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
