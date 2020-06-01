using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    Soundcontrollerscript soundController;

    private void Start()
    {
        soundController = GameObject.Find("Soundcontroller").GetComponent<Soundcontrollerscript>();
    }

    public void SceneLoad(int Scene_Number)
    {
        SceneManager.LoadScene(Scene_Number);
        if(Scene_Number >= 2) 
        {
            PlayerMovement.controllable = true;
        }
    }


    public void FadeToLevel() 
    {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneIndex += 1;
        if (SceneIndex.Equals(2)) {
            PlayerMovement.controllable = true;
        }
        else if (SceneIndex > 3 ) {
            PlayerMovement.abilities = true;
        }
        SceneManager.LoadScene(SceneIndex);

    }


    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.C))
        {
            FadeToLevel();
        }
        */
    }

    public void stopOnPause()
    {
        soundController.StopAudioSource();
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    public void Quit()
    {
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
