using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateCollectablesAmount : MonoBehaviour
{

    NoDestruction ProgressCollectables;

    public TextMeshProUGUI collectables_text;
    
    void Start()
    {
        ProgressCollectables = GameObject.Find("CollectablesProgress").GetComponent<NoDestruction>();
        UpdateCollectables();
    }

    private void UpdateCollectables() 
    
    {
        collectables_text.text = "You found " + ProgressCollectables.CollectablesAmount + " out of 25 collectables.Try to find them all!";

        if (ProgressCollectables.CollectablesAmount == 25)
        {
            collectables_text.text = "Congratulations! You found all out of 25 collectables, thank you for playing Anemoi.";
        }
    }
}
