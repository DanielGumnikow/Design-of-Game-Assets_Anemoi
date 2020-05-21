using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundScript : MonoBehaviour
{
    private SoundManager soundmanager;
    public Slider soundslider;
    public TextMeshProUGUI volumetext;

    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        soundmanager = GameObject.FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseMusic()
    {
        soundmanager.ToggleSound();
    }

    public void setVolume(float volume) // Sets the Volume of the Global Main Mix
    {
        if (volume == -25)
        {
            audioMixer.SetFloat("volume", -80);
            volumetext.text = "Volume 0";
        }
        else 
        { 
        audioMixer.SetFloat("volume", volume);
        volumetext.text = "Volume " + MapRange(volume, -25f, 0f, 0f, 100f);
        }

    }

    public float MapRange(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
