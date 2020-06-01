using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Spine.Unity.Examples;
using UnityEngine;

public class changeAnimation : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset down, falling, walking;

    public string currentState;
    public string previousState;
    public string currentAnimation;

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        Spine.TrackEntry animationEntry = skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale = timeScale;
        //animationEntry.Complete += AnimationEntry_Complete;
        //skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {

        if (state.Equals("Walking"))
        {
            SetAnimation(walking, true, 0.08f);

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 4)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }

            Soundcontrollerscript.soundInstance.playAudioSource(4);

        }
        else if (state.Equals("Down"))
        {
            SetAnimation(down, false, 0.5f);

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 7)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }

            Soundcontrollerscript.soundInstance.playAudioSource(7);

        }
        else if (state.Equals("Falling"))
        {
            SetAnimation(falling, false, 1f);

            if (Soundcontrollerscript.soundInstance.currAudioSourceIndex != 3)
            {
                Soundcontrollerscript.soundInstance.StopAudioSource();
            }

            //Soundcontrollerscript.soundInstance.playAudioSource(7);

        }

    }

    public void changeLevelAfterAnim()
    {
        GameObject levelchanger = GameObject.Find("LevelChanger");
        levelchanger.GetComponent<MainMenu>().FadeToLevel();
    }

    

}
