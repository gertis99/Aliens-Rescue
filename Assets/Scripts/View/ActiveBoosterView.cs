using System.Collections;
using System.Collections.Generic;
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

    public void ActivateBooster()
    {
        if (isActivated)
            isActivated = false;
        else
            isActivated = true;
    }
}
