using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController
{
    public delegate void WinChecked();
    public delegate void PointsChanged(int number, int color);
    public static event WinChecked OnWinChecked;
    public static event PointsChanged OnPointsChanged;


    private int condition = 1;
    public int[] colorPoints = new int[6];
    private bool win = false;

    private GameProgressionService gameProgression;
    private GameConfigService gameConfig;
    private AnalyticsGameService analytics;

    private LevelController levelController;

    public WinController(LevelController controller)
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        gameConfig = ServiceLocator.GetService<GameConfigService>();
        analytics = ServiceLocator.GetService<AnalyticsGameService>();
        levelController = controller;
        levelController.OnCheckedMatch += AddPoints;
        
        foreach(LevelInfo level in gameConfig.Levels)
        {
            if(level.Id == PlayerPrefs.GetInt("LevelToLoad", 1))
            {
                condition = level.Goal;
                break;
            }
        }
    }

    public void AddPoints(Alien element)
    {
        if (element == null)
            return;

        colorPoints[(int)element.GetElementType()]++;
        OnPointsChanged(colorPoints[(int)element.GetElementType()], (int)element.GetElementType());
        if(!win)
            CheckWin();
    }

    public void AddPoints(List<Alien> elements)
    {
        for(int i = 0; i < elements.Count; i++)
        {
            colorPoints[(int)elements[i].GetElementType()]++;
            OnPointsChanged(colorPoints[(int)elements[i].GetElementType()], (int)elements[i].GetElementType());
        }

        if(!win)
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
        win = true;
        analytics.SendEvent("finishLevel", new Dictionary<string, object> { ["levelId"] = PlayerPrefs.GetInt("LevelToLoad", -1) });
        OnWinChecked();
        if (gameProgression.CurrentLevel == PlayerPrefs.GetInt("LevelToLoad", 1))
            gameProgression.UpdateCurrentLevel(1);
        gameProgression.UpdateCurrency("Gold", coinsGained);
    }
}
