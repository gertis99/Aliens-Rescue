using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseController
{
    public delegate void LoseChecked();
    public static event LoseChecked OnLoseChecked;
    public int nMovements = 1;

    private LevelController levelController;
    private GameConfigService gameConfig;

    public static event Action OnMoveDone;

    public LoseController(LevelController controller)
    {
        levelController = controller;
        levelController.OnMoveDone += MoveDone;
        gameConfig = ServiceLocator.GetService<GameConfigService>();

        foreach (LevelInfo level in gameConfig.Levels)
        {
            if (level.Id == PlayerPrefs.GetInt("LevelToLoad", 1))
            {
                nMovements = level.Movements;
                break;
            }
        }
    }
    
    private void MoveDone()
    {
        nMovements--;
        OnMoveDone();
        if(nMovements <= 0)
        {
            OnLoseChecked();
        }
    }

}
