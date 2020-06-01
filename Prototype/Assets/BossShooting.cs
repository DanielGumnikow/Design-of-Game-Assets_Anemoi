using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    private float timeBetweenShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    private Transform player;
    private void Start()
    {
        timeBetweenShots = startTimeBtwShots;
    }

    private void Update()
    {
        if (timeBetweenShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            Debug.Log("transform Position: " + transform.position);

            timeBetweenShots = startTimeBtwShots;
        }
        else 
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}
