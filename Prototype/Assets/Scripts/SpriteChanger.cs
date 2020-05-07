using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public static SpriteChanger changerinstance;


    private void Awake()
    {

        changerinstance = this;

    }

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void changeSprite(Sprite otherSprite)
    {
        Debug.Log("sprite loaded succesfully: " + otherSprite);
        spriteRenderer.sprite = otherSprite;
    }
}
