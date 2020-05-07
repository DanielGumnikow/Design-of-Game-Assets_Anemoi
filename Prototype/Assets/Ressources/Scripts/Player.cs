using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int MaxHealthPoints = 3;
    public static int HealthPoints = 3;
    //public GameObject[] checkpoints;
    public GameObject lastCheckpoint;
    private Transform PlayerTransform;

    void Update()
    {
        if (HealthPoints == 0) {
            PlayerTransform = GameObject.Find("Player_Animated").transform;
            PlayerTransform.position = new Vector2(lastCheckpoint.transform.position.x, -2);
            HealthPoints = 3;
            SpriteChanger.changerinstance.changeSprite(Infoscript.instance.HealthSprites[0]);
            //Soundcontrollerscript.soundInstance.
        }
    }

    public void Respawn()
    {
        float checkpointPositionX = lastCheckpoint.transform.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            //Destroy(collision.gameObject);
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
        if (collision.tag == "BoostFlower")
        {
            //Debug.Log("here boost");
            PlayerMovement pM = GameObject.FindObjectOfType<PlayerMovement>();
            pM.getBoost();
            //PlayerMovement.
        }
        if (collision.tag == "Checkpoint")
        {
           
            lastCheckpoint = collision.gameObject;
            //Debug.Log("x coord: " + lastCheckpoint.transform.position.x);
        }
    }
}
