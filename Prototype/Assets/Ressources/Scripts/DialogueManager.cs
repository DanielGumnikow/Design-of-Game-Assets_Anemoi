using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    public GameObject LevelLoader;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;


    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue) {

        animator.SetBool("isOpen", true);
        //Debug.Log("Starting conversation with " + dialogue.name);

        sentences.Clear();

        foreach(string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();

    }

    IEnumerator TypeSentence (string sentence) 
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) 
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    void EndDialogue() 
    {
        //Debug.Log("End of Conversation");
        animator.SetBool("isOpen", false);
        MainMenu mM = LevelLoader.GetComponent<MainMenu>();
        mM.FadeToLevel();
    }
}
