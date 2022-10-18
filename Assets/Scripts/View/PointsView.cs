using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointsView : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI[] pointsText = new TMPro.TextMeshProUGUI[6];
    [SerializeField]
    private TMPro.TextMeshProUGUI[] pointsTextFinish = new TMPro.TextMeshProUGUI[6];
    private int[] points = new int[6];
    private int goldGained = 0;
    [SerializeField]
    private TMP_Text goldGainedText;
    [SerializeField]
    private GameObject win;
    [SerializeField]
    private TMPro.TextMeshProUGUI lose, movementsText;
    private int winCondition = 1, movements = 1, maxMovements = 1;
    [SerializeField]
    private Button nextLevelButton;

    private GameConfigService gameConfig;
    private GameProgressionService gameProgression;
    private PointsController pointsController;

    private void Awake()
    {
        gameConfig = ServiceLocator.GetService<GameConfigService>();
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    public void Initialize(PointsController controller)
    {
        pointsController = controller;

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

        pointsController.OnPointsChanged += AddPoint;
        pointsController.OnWinChecked += ActiveWin;
        pointsController.OnMoveDone += MoveDone;
        pointsController.OnLoseChecked += ActiveLose;
    }

    private void OnDisable()
    {
        pointsController.OnPointsChanged -= AddPoint;
        pointsController.OnWinChecked -= ActiveWin;
        pointsController.OnMoveDone -= MoveDone;
        pointsController.OnLoseChecked -= ActiveLose;
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
            goldGained += points[i] - winCondition;
        }

        if(gameConfig.ExistLevel(PlayerPrefs.GetInt("LevelToLoad", 1) + 1))
        {
            nextLevelButton.interactable = true;
        }
        else
        {
            nextLevelButton.interactable = false;
        }

        goldGainedText.text = goldGained.ToString();
        win.gameObject.SetActive(true);
    }

    private void MoveDone()
    {
        movements--;
        movementsText.text = movements.ToString();
    }

    private void ActiveLose()
    {
        for (int i = 0; i < pointsText.Length; i++)
        {
            pointsTextFinish[i].text = (points[i] - winCondition).ToString();
            if(points[i] - winCondition > 0)
                gameProgression.UpdateAliensRescued(i, points[i] - winCondition);
        }

        nextLevelButton.interactable = false;
        win.gameObject.SetActive(true);
    }

    public void LoadNextLevel()
    {
        nextLevelButton.GetComponent<ButtonLoadLevel>().LoadLevelScene(PlayerPrefs.GetInt("LevelToLoad", 1) + 1);
    }
}
