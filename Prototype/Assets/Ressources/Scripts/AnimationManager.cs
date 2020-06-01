using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator[] animationList;
    public Animator HealthSprite;


    public void playAnimation(int animationIndex) 
    {
        animationList[animationIndex].SetBool("playState1", true);
    }

    public void playHealthAnimation()
    {
        HealthSprite.SetBool("playState1", true);
    }
}
