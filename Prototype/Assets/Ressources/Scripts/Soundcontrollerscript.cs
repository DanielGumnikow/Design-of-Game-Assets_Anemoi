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
    public int currAudioSourceIndex;
    private void Awake()
    {
        soundInstance = this;
    }

    private void Update()
    {

    }

    public void stopMainMix()
    {
    }



    public void playAudioSource(int AudioSourceIndex)
    {
        if (!audioSource[AudioSourceIndex].isPlaying)
        {
            currAudioSourceIndex = AudioSourceIndex;
            currentSource = audioSource[AudioSourceIndex];
            currentSource.Play();
        }
    }

    public void StopAudioSource()
    {
        if (currentSource != null)
        {
            currentSource.Stop();            
            //currentSource.SetScheduledEndTime(currentSource.clip.length - 1f);

        }
    }


}

