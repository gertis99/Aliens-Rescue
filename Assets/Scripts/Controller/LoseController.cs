using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseController : MonoBehaviour
{
    public delegate void LoseChecked();
    public static event LoseChecked OnLoseChecked;
    public int nMovements = 15;

    // Start is called before the first frame update
    void Start()
    {
        SwapsController.OnMoveDone += MoveDone;
    }

    private void OnDisable()
    {
        SwapsController.OnMoveDone -= MoveDone;
    }

    private void MoveDone()
    {
        nMovements--;
        if(nMovements <= 0)
        {
            OnLoseChecked();
        }
    }

}
