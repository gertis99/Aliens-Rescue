using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveBoosterView : MonoBehaviour
{
    protected ActiveBoosterController controller;
    protected bool isActivated = false;
    public int boostersLeft;

    private void Awake()
    {
        controller = new ActiveBoosterController();
    }

    private void Start()
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = boostersLeft.ToString();   
    }

    public void ActivateBooster()
    {
        if (isActivated)
            isActivated = false;
        else
            isActivated = true;
    }
}
