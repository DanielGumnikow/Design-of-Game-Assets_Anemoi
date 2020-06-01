using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quit : MonoBehaviour
{
    void Start()
    {

    }

    public void QuitGame()
    {
    #if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
    #endif
    }
}
