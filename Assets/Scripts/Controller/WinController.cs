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

    public void AddPoints(Alien element)
    {
        colorPoints[(int)element.GetElementType()]++;
        OnPointsChanged(colorPoints[(int)element.GetElementType()], (int)element.GetElementType());
        CheckWin();
    }

    public void AddPoints(List<Alien> elements)
    {
        for(int i = 0; i < elements.Count; i++)
        {
            colorPoints[(int)elements[i].GetElementType()]++;
            OnPointsChanged(colorPoints[(int)elements[i].GetElementType()], (int)elements[i].GetElementType());
        }

        CheckWin();
    }

    private void CheckWin()
    {
        int coinsGained = 0;

        for(int i=0; i<colorPoints.Length; i++)
        {
            if (colorPoints[i] < condition)
                return;
            else
                coinsGained += colorPoints[i] - condition;
        }
        OnWinChecked();
        if (PlayerInfo.ActualLevel == level)
            PlayerInfo.ActualLevel++;
        PlayerInfo.Coins += coinsGained;
    }
}
