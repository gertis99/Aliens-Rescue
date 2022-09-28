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
    public int[] cosmeticsBought; // pos = id, number of items

    private IGameProgressionProvider progressionProvider;

    public void Initialize(GameConfigService gameConfig, IGameProgressionProvider progressionProvider)
    {
        this.progressionProvider = progressionProvider;
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

    public void UpdateCosmetics(int id, int number)
    {
        cosmeticsBought[id] += number;
        Save();
    }

    public void Save()
    {
        progressionProvider.Save(JsonUtility.ToJson(this));
    }

    public void Load(GameConfigService config)
    {
        string data = progressionProvider.Load();
        if (string.IsNullOrEmpty(data))
        {
            Gold = config.InitialGold;
            HorizontalLineBoosters = config.InitialHorizontalLineBooster;
            VerticalLineBoosters = config.InitialVerticalLineBooster;
            BombBoosters = config.InitialBombBooster;
            ColorBombBoosters = config.InitialColorBombBooster;
            CurrentLevel = 1;
            currentHUDColor = HUDColors.ORANGE;
            cosmeticsBought = new int[6];
            Debug.Log("CC");
            Save();
        }
        else
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }
        
    }
//end of save and load

    public int GetNumberItemsById(int id)
    {
        if (id < cosmeticsBought.Length)
            return cosmeticsBought[id];

        return 0;
    }

    public void Clear()
    {
    }
}