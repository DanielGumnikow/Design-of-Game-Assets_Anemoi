using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Projectile : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        //transform.position = Vector2.Lerp(transform.position, player.position, speed * Time.deltaTime);

        if (transform.position.x == (target.x - 2))
        {
            DestroyProjectile();
        }
        
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}
