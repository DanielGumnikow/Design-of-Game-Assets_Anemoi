using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Collections;

public class Infoscript : MonoBehaviour
{
    public Sprite[] HealthSprites;
    public Sprite[] AmuletteSprites;

    public GameObject HealtSpriteUI;
    public GameObject AmuletteSpriteUI;
    
    private Slider staminaBar;
    private Slider FloatBar;

    private WaitForSeconds regenTick = new WaitForSeconds(0.01f);
    public Coroutine regen;

    private WaitForSeconds regenTickDash = new WaitForSeconds(0.02f);
    public Coroutine regenDash;

    private WaitForSeconds regenTickFloat = new WaitForSeconds(0.01f);
    public Coroutine regenFloat;

    private WaitForSeconds regenTickHealthPoints = new WaitForSeconds(0.3f);
    public Coroutine regenHealthPoints;

    public static Infoscript instance;

    public Animator HealthSprite;

    private void Awake()
    {

        instance = this;

    }

    void Start()
    {
        PlayerMovement.JumpValue = PlayerMovement.MaxJumpValue;
        //staminaBar.value = PlayerMovement.MaxJumpValue;
        //staminaBar.value = PlayerMovement.JumpValue;

        PlayerMovement.FloatValue = PlayerMovement.MaxFloatValue;
        //FloatBar.value = PlayerMovement.MaxFloatValue;
        //FloatBar.value = PlayerMovement.FloatValue;

        PlayerMovement.DashValue = PlayerMovement.MaxDashValue;
        //DashBar.value = PlayerMovement.MaxDashValue;
        //DashBar.value = PlayerMovement.DashValue;

        Player.HealthPoints = Player.MaxHealthPoints;


    }

    public void Use_Stamina(float amount , float PlayerValue , Slider barindex, Coroutine c_routine, IEnumerator IEnum ) {
        if (PlayerValue - amount >= 0)
        {
            PlayerValue -= amount;
            barindex.value = PlayerValue;

            if (c_routine != null)
            {
                StopCoroutine(c_routine);
            }
            c_routine = StartCoroutine(IEnum);
        }
        else
        {
            //Debug.Log("Not enough JUMPStamina");
        }
    }

    public void UseStamina(float amount) {

        if (PlayerMovement.JumpValue - amount >= 0) {
            PlayerMovement.JumpValue -= amount;
            staminaBar.value = PlayerMovement.JumpValue;

            if (regen != null) {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(RegenStamina());       
        }
        else {
            //Debug.Log("Not enough JUMPStamina");
        }
    }

    /*
    public void UseFloat(float amount)
    {

        if (PlayerMovement.FloatValue - amount >= 0)
        {
            PlayerMovement.FloatValue -= amount;
            FloatBar.value = PlayerMovement.FloatValue;

            if (regenFloat != null)
            {
                StopCoroutine(regenFloat);
            }

            regenFloat = StartCoroutine(RegenFloat());
        }
        else
        {
            Debug.Log("Not enough FloatStamina");
        }
    }
    */

        /*
    public void UseDash(float amount)
    {

        if (PlayerMovement.DashValue - amount >= 0)
        {
            PlayerMovement.DashValue -= amount;
            DashBar.value = PlayerMovement.DashValue;

            if (regenDash != null)
            {
                StopCoroutine(regenDash);
            }

            regenDash = StartCoroutine(RegenDash());
        }
        else
        {
            Debug.Log("Not enough DashStamina");
        }
    }
    */

    public void AnimationEnded() 
    {
        HealthSprite.SetBool("playState1", false);
    }

    



    public void DamageHealthpoints(int amount)
    {
            Player.HealthPoints -= amount;
            UpdateHealthpoints();
    }

    public void UpdateHealthpoints()
    {
        if (Player.HealthPoints == 3)
        {
            HealtSpriteUI.GetComponent<SpriteRenderer>().sprite = HealthSprites[0];
            HealtSpriteUI.GetComponent<Animator>().SetBool("playState1", true);
        }
        else if (Player.HealthPoints == 2)
        {
            HealtSpriteUI.GetComponent<SpriteRenderer>().sprite = HealthSprites[1];
            HealtSpriteUI.GetComponent<Animator>().SetBool("playState1", true);
        }
        else if (Player.HealthPoints == 1)
        {
            HealtSpriteUI.GetComponent<SpriteRenderer>().sprite = HealthSprites[2];
            HealtSpriteUI.GetComponent<Animator>().SetBool("playState1", true);
        }
        else if (Player.HealthPoints == 0)
        {
            HealtSpriteUI.GetComponent<SpriteRenderer>().sprite = HealthSprites[3];
            HealtSpriteUI.GetComponent<Animator>().SetBool("playState1", true);
        }
    }

    public void UpdateDashAmulette()
    {
        if (AmuletteSpriteUI != null) {
        if (Player.instancePlayer.currDash == 3)
        {
            AmuletteSpriteUI.GetComponent<SpriteRenderer>().sprite = AmuletteSprites[3];
            AmuletteSpriteUI.GetComponent<Animator>().SetBool("playState1", true);
        }
        else if (Player.instancePlayer.currDash == 2)
        {
            AmuletteSpriteUI.GetComponent<SpriteRenderer>().sprite = AmuletteSprites[2];
            AmuletteSpriteUI.GetComponent<Animator>().SetBool("playState1", true);
        }
        else if (Player.instancePlayer.currDash == 1)
        {
            AmuletteSpriteUI.GetComponent<SpriteRenderer>().sprite =  AmuletteSprites[1];
            AmuletteSpriteUI.GetComponent<Animator>().SetBool("playState1", true);
        }
        else if (Player.instancePlayer.currDash == 0)
        {
            AmuletteSpriteUI.GetComponent<SpriteRenderer>().sprite = AmuletteSprites[0];
            AmuletteSpriteUI.GetComponent<Animator>().SetBool("playState1", true);
        }
        }
        else
        {
            // no Amulette
        }
    }

    void Update()
    {
        //JumpText.text = "Jump: " + PlayerMovement.JumpValue;
    }


    public IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1f);

        while (PlayerMovement.JumpValue < PlayerMovement.MaxJumpValue) {
            PlayerMovement.JumpValue += PlayerMovement.MaxJumpValue / 100;
            instance.staminaBar.value = PlayerMovement.JumpValue;
            yield return instance.regenTick;
        }
        instance.regen = null;
    }
    public IEnumerator RegenDash()
    {
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("nixpassiert");

        while (Player.instancePlayer.currDash < Player.instancePlayer.maxDash)
        {
            Player.instancePlayer.currDash += Player.instancePlayer.maxDash / 3;
            instance.UpdateDashAmulette();
            yield return instance.regenTickDash;
        }
        instance.regenDash = null;
    }
    public IEnumerator RegenFloat()
    {
        yield return new WaitForSeconds(2);

        
        while (PlayerMovement.FloatValue < PlayerMovement.MaxFloatValue)
        {
            PlayerMovement.FloatValue += PlayerMovement.MaxFloatValue / 100;
            instance.FloatBar.value = PlayerMovement.FloatValue;
            yield return instance.regenTickFloat;
        }
        instance.regenFloat = null;
    }

    /*
    public IEnumerator RegenHealthPoints()
    {
        yield return new WaitForSeconds(5f);

        while (Player.HealthPoints < Player.MaxHealthPoints)
        {
            Player.HealthPoints += Player.MaxHealthPoints / 3;
            instance.HealthBar.value = Player.HealthPoints;
            if (Player.HealthPoints == 3)
            {
                SpriteChanger.changerinstance.changeSprite(HealthSprites[0]);
            }
            else if (Player.HealthPoints == 2)
            {
                HealthSprite.GetComponent<Image>().sprite = HealthSprites[1];
            }
            else if (Player.HealthPoints == 1)
            {
                HealthSprite.GetComponent<Image>().sprite = HealthSprites[2];
            }

            yield return instance.regenTickHealthPoints;


        }
        instance.regenHealthPoints = null;
    }
    */

}
