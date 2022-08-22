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


    public int condition;
    public int level;   
    public int[] colorPoints = new int[6];

    private void Awake()
    {
        LevelController.OnCheckedMatch += AddPoints;
    }

    private void OnDisable()
    {
        LevelController.OnCheckedMatch -= AddPoints;
    }

    public void AddPoints(Element element)
    {
        if(element.GetColorType() < colorPoints.Length)
        {
            colorPoints[element.GetColorType()]++;
            OnPointsChanged(colorPoints[element.GetColorType()], element.GetColorType());
            CheckWin();
        }
        
    }

    public void AddPoints(List<Element> elements)
    {
        for(int i = 0; i < elements.Count; i++)
        {
            if (elements[i].GetColorType() < colorPoints.Length)
            {
                colorPoints[elements[i].GetColorType()]++;
                OnPointsChanged(colorPoints[elements[i].GetColorType()], elements[i].GetColorType());
            }
                
        }

        CheckWin();
    }

    private void CheckWin()
    {
        for(int i=0; i<colorPoints.Length; i++)
        {
            if (colorPoints[i] < condition)
                return;
        }
        OnWinChecked();
        if (PlayerInfo.ActualLevel == level)
            PlayerInfo.ActualLevel++;
    }
}
