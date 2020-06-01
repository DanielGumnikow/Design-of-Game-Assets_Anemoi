using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    public GameObject mainCam;

    float shakeAmount = 0;

    CinemachineVirtualCamera virtualcam;
    GameObject Player;

    private void Awake()
    {
        if (mainCam == null)
        {
            //mainCam = Camera.main;
        }
    }

    private void Start()
    {
        Player = GameObject.Find("Player_Animated");
        virtualcam = mainCam.GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void BeginShake()
    {
        if (shakeAmount > 0) 
        {
            
            virtualcam.m_Follow = null;
            Vector3 camPos = mainCam.transform.position;


            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;

        }
    }
    void StopShake()
    {
        CancelInvoke("BeginShake");
        virtualcam.m_Follow = Player.transform;
        mainCam.transform.localPosition = Vector3.zero;
    }
}

