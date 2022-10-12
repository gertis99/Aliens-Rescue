using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsView : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] pointsText = new TMPro.TextMeshProUGUI[6];
    public TMPro.TextMeshProUGUI[] pointsTextFinish = new TMPro.TextMeshProUGUI[6];
    private int[] points = new int[6];
    public GameObject win;
    public TMPro.TextMeshProUGUI lose, movementsText;
    public int winCondition = 1, movements = 1, maxMovements = 1;

    private GameConfigService gameConfig;
    private GameProgressionService gameProgression;

    private void Awake()
    {
        gameConfig = ServiceLocator.GetService<GameConfigService>();
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    private void Start()
    {

        foreach (LevelInfo level in gameConfig.Levels)
        {
            if (level.Id == PlayerPrefs.GetInt("LevelToLoad", 1))
            {
                maxMovements = level.Movements;
                winCondition = level.Goal;
                break;
            }
        }

        for (int i=0; i < pointsText.Length; i++)
        {
            pointsText[i].text = 0 + "/" + winCondition;
        }

        movements = maxMovements;

        movementsText.text = movements.ToString(); 

        WinController.OnPointsChanged += AddPoint;
        WinController.OnWinChecked += ActiveWin;
        LoseController.OnMoveDone += MoveDone;
        LoseController.OnLoseChecked += ActiveLose;
    }

    private void OnDisable()
    {
        WinController.OnPointsChanged -= AddPoint;
        WinController.OnWinChecked -= ActiveWin;
        LoseController.OnMoveDone -= MoveDone;
        LoseController.OnLoseChecked -= ActiveLose;
    }

    private void AddPoint(int number, int color)
    {
        pointsText[color].text = number + "/" + winCondition;
        points[color] = number;
    }

    private void ActiveWin()
    {
        for(int i = 0; i < pointsText.Length; i++)
        {
            pointsTextFinish[i].text = (points[i] - winCondition).ToString();
            gameProgression.UpdateAliensRescued(i, points[i] - winCondition);
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
