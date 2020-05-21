using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static int MaxHealthPoints = 3;
    public static int HealthPoints = 3;
    //public GameObject[] checkpoints;
    public GameObject lastCheckpoint;
    private Transform PlayerTransform;
    public static int maxDash = 0;
    public static int currDash = 0;
    public GameObject dialogueBoxM;
    public TextMeshProUGUI collectablesAmount;
    private int currCollectables = 0;

    public static Player instancePlayer;

    private void Awake()
    {
        instancePlayer = this;
    }
    void Update()
    {
        checkHP();
    }

    public int getCurrDash()
    {
        return currDash;
    }

    private void checkHP()
    {
        if (HealthPoints == 0)
        {
            Respawn();
        }
    }

    public void addHP()
    {
        if (HealthPoints > 0 && HealthPoints < 3)
        {
            HealthPoints += 1;
            Infoscript.instance.UpdateHealthpoints();
        }
    }

    private void fillHP()
    {
        HealthPoints = 3;
        SpriteChanger.changerinstance.changeSprite(Infoscript.instance.HealthSprites[0]);
    }

    public void Respawn()
    {
        fillHP();
        PlayerTransform = GameObject.Find("Player_Animated").transform;
        PlayerTransform.position = new Vector2(lastCheckpoint.transform.position.x, lastCheckpoint.transform.position.y);
        float checkpointPositionX = lastCheckpoint.transform.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
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
            dialogueBoxM.SetActive(true);      
            dTrigger.TriggerDialogue();     
            dTrigger.ActivateButton();
            dTrigger.DeactivateDialogue();

        }
        if (collision.tag == "BoostFlower")
        {
            PlayerMovement pM = GameObject.FindObjectOfType<PlayerMovement>();
            pM.getBoost();
        }
        if (collision.tag == "Checkpoint")
        {      
            lastCheckpoint = collision.gameObject;
        }

        if (collision.tag == "Killzone")
        {
            Respawn();
        }
        if (collision.tag == "EndGame")
        {
            GameObject levelchanger = GameObject.Find("LevelChanger");
            levelchanger.GetComponent<MainMenu>().FadeToLevel();
        }
        if (collision.tag == "HealthItem")
        {
            if(HealthPoints < 3)
            {
            addHP();
            Destroy(collision.gameObject);
            }
        }
        if (collision.tag == "Collectable")
        {
            currCollectables += 1;
            collectablesAmount.text = currCollectables + " / 25";
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Amulette")
        {
            maxDash += 1;
            currDash += 1;
            Infoscript.instance.UpdateDashAmulette();
            Destroy(collision.gameObject);
        }


    }
}
