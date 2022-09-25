using System;
using System.Collections.Generic;
using UnityEngine;

    
[System.Serializable]
public class GameProgressionService : IService
{
    public int Gold;
    public int CurrentLevel;
    public int HorizontalLineBoosters;
    public int VerticalLineBoosters;
    public int BombBoosters;
    public int ColorBombBoosters;
    public HUDColors currentHUDColor;

    //public event Action OnInventoryChanged;

    public void Initialize(GameConfigService gameConfig)
    {
        Load(gameConfig);
    }

    public void UpdateGold(int amount)
    {
        Gold += amount;
        Save();
    }

    public void UpdateHorizontalLineBoosters(int amount)
    {
        HorizontalLineBoosters += amount;
        //OnInventoryChanged?.Invoke();
        Save();
    }

    public void UpdateVerticalLineBoosters(int amount)
    {
        VerticalLineBoosters += amount;
        Save();
    }

    public void UpdateBombBoosters(int amount)
    {
        BombBoosters += amount;
        Save();
    }

    public void UpdateColorBombBoosters(int amount)
    {
        ColorBombBoosters += amount;
        Save();
    }

    public void UpdateCurrentLevel(int amount)
    {
        CurrentLevel += amount;
        Save();
    }

    public void UpdateHUDColor(HUDColors color)
    {
        currentHUDColor = color;
        Save();
    }

    //save and load
    private static string kSavePath = "/gameProgression.json";

    public void Save()
    {
        System.IO.File.WriteAllText(Application.persistentDataPath + kSavePath, JsonUtility.ToJson(this));
        Debug.Log("DD");
    }

    public void Load(GameConfigService config)
    {
        Debug.Log("AA");
        if (System.IO.File.Exists(Application.persistentDataPath + kSavePath))
        {
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(Application.dataPath + kSavePath),
                this);
            return;
        }

        Debug.Log("BB");
        Gold = config.InitialGold;
        HorizontalLineBoosters = config.InitialHorizontalLineBooster;
        VerticalLineBoosters = config.InitialVerticalLineBooster;
        BombBoosters = config.InitialBombBooster;
        ColorBombBoosters = config.InitialColorBombBooster;
        CurrentLevel = 1;
        currentHUDColor = HUDColors.ORANGE;
        Debug.Log("CC");
        Save();
    }
//end of save and load

    public void Clear()
    {
    }
}