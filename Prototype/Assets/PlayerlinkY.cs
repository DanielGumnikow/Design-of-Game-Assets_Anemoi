using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerlinkY : MonoBehaviour
{

    private Transform Link;
    private Transform playertransform;
    // Start is called before the first frame update
    void Start()
    {
        playertransform = GameObject.FindGameObjectWithTag("Player").transform;
        Link = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Link.position = new Vector2(transform.position.x, playertransform.position.y + 2);
    }


}
