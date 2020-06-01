using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public static int bossHP = 3;

    public Animator BossHealthBar;
    public Slider HealthBarSlider;

    public static Boss bossInstance;

    private void Awake()
    {
        bossInstance = this;
    }
    public void changeBossHP()
    {
        if (bossHP > 0)
        {
            bossHP -= 1;
            BossHealthBar.SetBool("playState1", true);
            HealthBarSlider.value -= 1;
            if (bossHP == 0)
            {
                GameObject fillarea = GameObject.Find("Fill Area");
                fillarea.SetActive(false);
                BossHealthBar.SetBool("dead", true);
                GameObject ShootingLogic = GameObject.Find("ShootingLogic");
                ShootingLogic.gameObject.SetActive(false);
            }
        }
    }
}
