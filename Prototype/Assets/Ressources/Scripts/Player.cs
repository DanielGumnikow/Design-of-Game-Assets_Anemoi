using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int MaxHealthPoints = 3;
    public static int HealthPoints = 3;

    void Update()
    {
        if (HealthPoints == 0) {
            print("lost");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Infoscript.instance.DamageHealthpoints(1);
            Soundcontrollerscript.soundInstance.playAudioSource(1);
        }

        if (collision.tag == "TriggerEvent")
        {
            Destroy(collision.gameObject);
            GameObject levelchanger = GameObject.Find("LevelChanger");
            levelchanger.GetComponent<MainMenu>().FadeToLevel();
        }

        if (collision.tag == "DialogueTrigger")
        {
            DialogueTrigger dTrigger = collision.GetComponent<DialogueTrigger>();
            dTrigger.TriggerDialogue();
            dTrigger.ActivateButton();
            //Destroy(collision.gameObject);
        }
    }
}
