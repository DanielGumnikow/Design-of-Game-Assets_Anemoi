using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageScript : MonoBehaviour
{
    [SerializeField]
    private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField]
    private float alphaSet = 0.8f;
    private float alphaMultiplier = 0.85f;

    private Transform player;

    private SkeletonAnimation SR;
    private SkeletonAnimation playerSR;

    private Color color;

    private void OnEnable()
    {
        SR = GetComponent<SkeletonAnimation>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponent<SkeletonAnimation>();

        alpha = alphaSet;
        SR = playerSR;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;

    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        //SR.color = color;

        if (Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
