using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public delegate void WinChecked();
    public delegate void PointsChanged(int number, int color);
    public static event WinChecked OnWinChecked;
    public static event PointsChanged OnPointsChanged;


    public int condition = 20;
    private int[] colorPoints = new int[6];

    private void Awake()
    {
        SwapsController.OnCheckedMatch += AddPoints;
    }

    private void OnDisable()
    {
        SwapsController.OnCheckedMatch -= AddPoints;
    }

    private void AddPoints(Element element)
    {
        colorPoints[element.GetColorType()]++;
        OnPointsChanged(colorPoints[element.GetColorType()], element.GetColorType());
    }

    private void CheckWin()
    {
        for(int i=0; i<colorPoints.Length; i++)
        {
            if (colorPoints[i] < condition)
                return;
        }
        OnWinChecked();
    }
}
