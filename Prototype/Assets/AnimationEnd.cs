using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnd : MonoBehaviour
{
    public Animator Animator;

    public void AnimationEnded() 
    {
        Animator.SetBool("playState1", false);
    }


}
