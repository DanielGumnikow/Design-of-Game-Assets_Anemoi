using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{

    public GameObject defaultSprite;
    public static SpriteChanger changerinstance;
    //private SpriteRenderer spriteR;
    private Sprite[] sprites;

    public Sprite btn_Image;
    public Sprite btn_HL_Image;

    public GameObject amuletteref;
    private void Awake()
    {

        changerinstance = this;

    }

    private void Start()
    {
        //changeHLSprite();
        //spriteR = gameObject.GetComponent<SpriteRenderer>();
        //sprites = Resources.LoadAll<Sprite>("");
        //HealthImage.sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void changeSprite(Sprite otherSprite)
    {
        //spriteR.sprite = otherSprite;
        defaultSprite.GetComponent<SpriteRenderer>().sprite = otherSprite;
    }

    public void AnimationEndedAmulette()
    {
        amuletteref.GetComponent<Animator>().SetBool("playState1", false);
    }
}
