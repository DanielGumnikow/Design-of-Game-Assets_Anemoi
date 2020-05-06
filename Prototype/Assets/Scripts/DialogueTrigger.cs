using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private void Start()
    {
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

    public IEnumerator TriggerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        TriggerDialogue();


    }
}