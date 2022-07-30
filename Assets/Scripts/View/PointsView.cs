using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsView : MonoBehaviour
{
    public Text[] pointsText = new Text[6];
    public Text win, lose, movementsText;
    public int winCondition = 20, movements = 15, maxMovements = 15;

    private void Start()
    {
        for(int i=0; i < pointsText.Length; i++)
        {
            pointsText[i].text = 0 + "/" + winCondition;
        }

        movementsText.text = "Movements: " + movements + "/" + maxMovements; 

        WinController.OnPointsChanged += AddPoint;
        WinController.OnWinChecked += ActiveWin;
        SwapsController.OnMoveDone += MoveDone;
        LoseController.OnLoseChecked += ActiveLose;
    }

    private void OnDisable()
    {
        WinController.OnPointsChanged -= AddPoint;
        WinController.OnWinChecked -= ActiveWin;
        SwapsController.OnMoveDone -= MoveDone;
        LoseController.OnLoseChecked -= ActiveLose;
    }

    private void AddPoint(int number, int color)
    {
        pointsText[color].text = number + "/" + winCondition;
    }

    private void ActiveWin()
    {
        win.gameObject.SetActive(true);
    }

    private void MoveDone()
    {
        movements--;
        movementsText.text = "Movements: " + movements + "/" + maxMovements;
    }

    private void ActiveLose()
    {
        lose.gameObject.SetActive(true);
    }
}
