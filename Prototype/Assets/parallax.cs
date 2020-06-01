using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class parallax : MonoBehaviour
{
    private float offset;

    [SerializeField]
    private float speed;

    GameObject virtualCam;
    Rigidbody2D playerRigidBody;
    GameObject player;
    PlayerMovement playerMovement;

    private float startX;

    private void Start()
    {
        
        virtualCam = GameObject.Find("CM vcam1");
        player = GameObject.Find("Player_Animated");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        startX = player.transform.position.x;

    }
    private void Update()
    {
        
        float tempY = virtualCam.transform.position.y;
        if (playerRigidBody.velocity.x != 0) {
        offset = (startX - player.transform.position.x) * speed * 2;
        transform.position = new Vector2(transform.position.x + offset, transform.position.y);
        }
        startX = player.transform.position.x;

    }

    /*
     *  float tempY = virtualCam.transform.position.y;
       //playerRigidBody.;
        if (playerRigidBody.velocity.x != 0 && tempX != player.transform.position.x) {
        offset = playerRigidBody.velocity.x * speed * -0.1f;
        //Debug.Log("Offset: "+ offset);
        //GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        transform.position = new Vector2(transform.position.x + offset, transform.position.y);
        }
        tempX = player.transform.position.x;
     * 
     * 
     * */
}
