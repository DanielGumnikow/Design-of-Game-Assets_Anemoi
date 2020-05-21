using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject button;
    public GameObject dialogueboxM;
    public DialogueManager dManager;
    private int SceneIndex;

    private void Start()
    {
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        //TriggerDialogue();
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    private void Update()
    {
        if (dialogue.trigger == true)
        {
            dialogue.trigger = false;
            StartCoroutine(TriggerCoroutine());
        }
    }

    public void ActivateButton()
    {
        dManager.DisplayNextSentence();

        if (SceneIndex < 4)
        {
            button.SetActive(true);
        }

    }

    public IEnumerator TriggerCoroutineOff()
    {
        yield return new WaitForSeconds(3f);
        dialogueboxM.SetActive(false);
    }

    public void DeactivateDialogue()
    {
        StartCoroutine(TriggerCoroutineOff());
        

    }


    public IEnumerator TriggerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        TriggerDialogue();


    }
}