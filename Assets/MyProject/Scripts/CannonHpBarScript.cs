using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonHpBarScript : MonoBehaviour
{
    private GameObject cannon;
    private Image HpBar;
    private int startHP;

    private void Awake()
    {
        HpBar = GetComponent<Image>();
        cannon = GameObject.Find("Cannon");
        startHP = cannon.GetComponent<CannonFollow>().HP;
    }

    private void FixedUpdate()
    {
        HpBar.fillAmount = (float)cannon.GetComponent<CannonFollow>().HP / startHP;
    }
}
