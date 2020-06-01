using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestruction : MonoBehaviour
{
    // Start is called before the first frame update
    public bool dontDestroy;
    public bool destroyPrev;

    public int CollectablesAmount;

    private void Awake()
    {
        if (dontDestroy == true)
        {
        DontDestroyOnLoad(this);
        }

        if (destroyPrev == true)
        {
            Destroy(GameObject.Find("GameplayLoopSource1"));
        }
    }

    public void updateCollactablesAmount() 
    {
        CollectablesAmount = Player.instancePlayer.currCollectables;
    }

}
