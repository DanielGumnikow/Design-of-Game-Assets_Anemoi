using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTransform : MonoBehaviour
{
    public GameObject[] Layers;

    private float startPositionX;

    private void Start()
    {
        startPositionX = Layers[1].transform.position.x;
    }

    private void Update()
    {
        checkForMove();
    }

    private void checkForMove()
    {
        if (Layers[1].transform.position.x == startPositionX - 86.72f)
        {
            Layers[0].transform.position = new Vector2(86.72f, Layers[0].transform.position.y);
            GameObject temp0 = Layers[0];
            GameObject temp1 = Layers[1];
            Layers[2] = temp0;
            Layers[1] = Layers[2];
            Layers[0] = temp1;

            startPositionX = Layers[1].transform.position.x;

        }
        else if (Layers[1].transform.position.x == startPositionX + 86.72) 
        {
            Layers[2].transform.position = new Vector2(-86.72f, Layers[0].transform.position.y);

            GameObject temp0 = Layers[0];
            GameObject temp1 = Layers[1];
            Layers[1] = temp0;
            Layers[0] = Layers[2];
            Layers[2] = temp1;

            startPositionX = Layers[1].transform.position.x;
        }
    }

}
