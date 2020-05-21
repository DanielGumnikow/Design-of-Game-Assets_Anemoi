using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Collections;

public class Infoscript : MonoBehaviour
{
    public Sprite[] HealthSprites;
    public Sprite[] AmuletteSprites;

    private Sprite tempSprite;
    
    private Slider staminaBar;
    private Slider FloatBar;
    private Slider DashBar;
    private Slider HealthBar;

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

    private GameObject amuletteref;
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

        amuletteref = GameObject.Find("amulette");

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
            Debug.Log("Not enough JUMPStamina");
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
            Debug.Log("Not enough JUMPStamina");
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
            SpriteChanger.changerinstance.changeSprite(HealthSprites[0]);
            HealthSprite.SetBool("playState1", true);
        }
        else if (Player.HealthPoints == 2)
        {
            SpriteChanger.changerinstance.changeSprite(HealthSprites[1]);
            HealthSprite.SetBool("playState1", true);
        }
        else if (Player.HealthPoints == 1)
        {
            SpriteChanger.changerinstance.changeSprite(HealthSprites[2]);
            HealthSprite.SetBool("playState1", true);
        }
        else if (Player.HealthPoints == 0)
        {
            SpriteChanger.changerinstance.changeSprite(HealthSprites[3]);
            HealthSprite.SetBool("playState1", true);
        }
    }

    public void UpdateDashAmulette()
    {
        if (Player.currDash == 3)
        {
            amuletteref.GetComponent<SpriteChanger>().changeSprite(AmuletteSprites[3]);
            amuletteref.GetComponent<Animator>().SetBool("playState1", true);
        }
        else if (Player.currDash == 2)
        {
            amuletteref.GetComponent<SpriteChanger>().changeSprite(AmuletteSprites[2]);
            amuletteref.GetComponent<Animator>().SetBool("playState1", true);
        }
        else if (Player.currDash == 1)
        {
            amuletteref.GetComponent<SpriteChanger>().changeSprite(AmuletteSprites[1]);
            amuletteref.GetComponent<Animator>().SetBool("playState1", true);
        }
        else if (Player.currDash == 0)
        {
            amuletteref.GetComponent<SpriteChanger>().changeSprite(AmuletteSprites[0]);
            amuletteref.GetComponent<Animator>().SetBool("playState1", true);
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
        Debug.Log("nixpassiert");

        while (Player.currDash < Player.maxDash)
        {
            Player.currDash += Player.maxDash / 3;
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
