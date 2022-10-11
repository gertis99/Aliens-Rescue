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

    private GameProgressionService gameProgression;
    private GameConfigService gameConfig;

    private LevelController levelController;

    public WinController(LevelController controller)
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        gameConfig = ServiceLocator.GetService<GameConfigService>();
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
        if (gameProgression.CurrentLevel == PlayerPrefs.GetInt("LevelToLoad", 1))
            gameProgression.UpdateCurrentLevel(1);
        gameProgression.UpdateCurrency("Gold", coinsGained);
    }
}
