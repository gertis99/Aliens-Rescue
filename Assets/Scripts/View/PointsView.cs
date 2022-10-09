using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsView : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] pointsText = new TMPro.TextMeshProUGUI[6];
    public TMPro.TextMeshProUGUI[] pointsTextFinish = new TMPro.TextMeshProUGUI[6];
    public GameObject win;
    public TMPro.TextMeshProUGUI lose, movementsText;
    public int winCondition = 20, movements = 15, maxMovements = 15;

    private void Start()
    {
        for(int i=0; i < pointsText.Length; i++)
        {
            pointsText[i].text = 0 + "/" + winCondition;
        }

        movementsText.text = movements.ToString(); 

        WinController.OnPointsChanged += AddPoint;
        WinController.OnWinChecked += ActiveWin;
        LevelController.OnMoveDone += MoveDone;
        LoseController.OnLoseChecked += ActiveLose;
    }

    private void OnDisable()
    {
        WinController.OnPointsChanged -= AddPoint;
        WinController.OnWinChecked -= ActiveWin;
        LevelController.OnMoveDone -= MoveDone;
        LoseController.OnLoseChecked -= ActiveLose;
    }

    private void AddPoint(int number, int color)
    {
        pointsText[color].text = number + "/" + winCondition;
    }

    private void ActiveWin()
    {
        for(int i = 0; i < pointsText.Length; i++)
        {
            pointsTextFinish[i].text = pointsText[i].text;
        }
        win.gameObject.SetActive(true);
    }

    private void MoveDone()
    {
        movements--;
        movementsText.text = movements.ToString();
    }

    private void ActiveLose()
    {
        lose.gameObject.SetActive(true);
    }
}
