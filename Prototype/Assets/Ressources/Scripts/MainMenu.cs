using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;


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
        Debug.Log("SceneIndex" + SceneIndex);
        Debug.Log("controllabe" + PlayerMovement.controllable);
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            FadeToLevel();
        }
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
