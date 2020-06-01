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

    [SerializeField]
    public int maxDash = 0;

    public int currDash = 0;
    public GameObject dialogueBoxM;
    public TextMeshProUGUI collectablesAmount;
    public int currCollectables = 0;

    public static Player instancePlayer;
    private ProgressSave progressManager;

    cameraShake cameraShake;
    NoDestruction ProgressCollectables;
    private void Awake()
    {
        instancePlayer = this;
    }
    private void Start()
    {
        //progressManager = GameObject.Find("ProgressManager").GetComponent<ProgressSave>();

        //HealthPoints = progressManager.playerHP;
        //maxDash = progressManager.playerMaxdash;
        currDash = maxDash;

        //Infoscript.instance.UpdateHealthpoints();
        //Infoscript.instance.UpdateDashAmulette();
        cameraShake = GameObject.Find("CameraControllerContainer").GetComponent<cameraShake>();

        ProgressCollectables = GameObject.Find("CollectablesProgress").GetComponent<NoDestruction>();
        Infoscript.instance.UpdateDashAmulette();

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
            //updateProgressHP();
            Infoscript.instance.UpdateHealthpoints();
        }
    }

    private void fillHP()
    {
        HealthPoints = 3;
        //updateProgressHP();
        Infoscript.instance.HealtSpriteUI.GetComponent<SpriteRenderer>().sprite = Infoscript.instance.HealthSprites[0];
    }

    public void Respawn()
    {
        fillHP();
        PlayerTransform = GameObject.Find("Player_Animated").transform;
        PlayerTransform.position = new Vector2(lastCheckpoint.transform.position.x, lastCheckpoint.transform.position.y);
        float checkpointPositionX = lastCheckpoint.transform.position.x;
        currDash = maxDash;
        Infoscript.instance.UpdateDashAmulette();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Infoscript.instance.DamageHealthpoints(1);
            cameraShake.Shake(0.4f, 0.1f);
            Soundcontrollerscript.soundInstance.playAudioSource(1);
        }

        if (collision.tag == "BoostFlower")
        {
            PlayerMovement pM = GameObject.FindObjectOfType<PlayerMovement>();
            pM.getBoost();
        }

        if (collision.tag == "TriggerEvent")
        {
            Destroy(collision.gameObject);
            GameObject levelchanger = GameObject.Find("LevelChanger");
            levelchanger.GetComponent<MainMenu>().FadeToLevel();
        }

        if (collision.tag == "DeactivateObject")
        {
            this.gameObject.SetActive(false);
        }

        if (collision.tag == "DialogueTrigger")
        {
            DialogueTrigger dTrigger = collision.GetComponent<DialogueTrigger>();
            dialogueBoxM.SetActive(true);      
            dTrigger.TriggerDialogue();     
            dTrigger.ActivateButton();
            dTrigger.DeactivateDialogue();

        }
        
        if (collision.tag == "Checkpoint")
        {      
            lastCheckpoint = collision.gameObject;
        }

        if (collision.tag == "Killzone")
        {
            cameraShake.Shake(0.4f, 0.1f);
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
            ProgressCollectables.updateCollactablesAmount();
            collectablesAmount.text = currCollectables + " / 25";
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Amulette")
        {
            maxDash += 1;
            currDash += 1;

            //updateProgressMaxDash();

            Infoscript.instance.UpdateDashAmulette();
            Destroy(collision.gameObject);
        }

        if (collision.tag == "BossSoul")
        {
            Boss.bossInstance.changeBossHP();
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Projectile")
        {
            cameraShake.Shake(0.4f, 0.1f);
            Infoscript.instance.DamageHealthpoints(1);
            Soundcontrollerscript.soundInstance.playAudioSource(1);
            Destroy(collision.gameObject);
        }


    }

    private void updateProgressHP()
    {
        progressManager.playerHP = HealthPoints;
    }

    private void updateProgressMaxDash()
    {
        progressManager.playerMaxdash = maxDash;
    }
}
