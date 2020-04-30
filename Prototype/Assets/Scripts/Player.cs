using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int MaxHealthPoints = 100;
    public static int HealthPoints;

    void Update()
    {
        if (HealthPoints == 0) {
            print("lost");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Infoscript.instance.DamageHealthpoints(10);
        }
    }
}
