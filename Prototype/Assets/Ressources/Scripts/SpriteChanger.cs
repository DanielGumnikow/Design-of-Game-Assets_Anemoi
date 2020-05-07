using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{

    public GameObject HealthImage;
    public static SpriteChanger changerinstance;
    //private SpriteRenderer spriteR;
    private Sprite[] sprites;

    private void Awake()
    {

        changerinstance = this;

    }

    private void Start()
    {
        //spriteR = gameObject.GetComponent<SpriteRenderer>();
        //sprites = Resources.LoadAll<Sprite>("");
        //HealthImage.sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void changeSprite(Sprite otherSprite)
    {
        //spriteR.sprite = otherSprite;
        HealthImage.GetComponent<SpriteRenderer>().sprite = otherSprite;
    }
}
