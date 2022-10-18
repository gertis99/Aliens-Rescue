using System;
using System.Collections.Generic;
using UnityEngine;

    
[System.Serializable]
public class GameProgressionService : IService
{
    public int CurrentLevel;
    public List<ActiveBoosterItemModel> ActiveBoosters = new List<ActiveBoosterItemModel>();
    public List<CosmeticItemModel> Cosmetics = new List<CosmeticItemModel>();
    public List<ColorHudItemModel> HudColors = new List<ColorHudItemModel>();
    public string currentHudColor;
    public List<InGameCurrency> Currencies = new List<InGameCurrency>();
    public List<AlienModel> AliensRescued = new List<AlienModel>();

    private IGameProgressionProvider progressionProvider;

    public void Initialize(GameConfigService gameConfig, IGameProgressionProvider progressionProvider)
    {
        this.progressionProvider = progressionProvider;
        Debug.Log("1");
        Load(gameConfig);
    }

    public void UpdateActiveBooster(string name, int amount)
    {
        foreach(ActiveBoosterItemModel boosterItem in ActiveBoosters)
        {
            if(boosterItem.Name == name)
            {
                boosterItem.Amount += amount;
                Save();
                return;
            }
        }

        ActiveBoosters.Add(new ActiveBoosterItemModel { Name = name, Amount = amount });
        Save();
    }

    public void UpdateCurrentLevel(int amount)
    {
        CurrentLevel += amount;
        Save();
    }

    public void UpdateColorHud(string name, bool selected)
    {
        foreach (ColorHudItemModel colorHudItem in HudColors)
        {
            if (colorHudItem.Name == name)
            {
                colorHudItem.Selected = selected;
                currentHudColor = colorHudItem.Name;
                Save();
                return;
            }
        }

        HudColors.Add(new ColorHudItemModel { Name = name, Selected = selected });
        currentHudColor = name;
        Save();
    }

    public void UpdateCosmetic(string name, int amountPurchased, int amountAvailable)
    {
        foreach (CosmeticItemModel cosmeticItem in Cosmetics)
        {
            if (cosmeticItem.Name == name)
            {
                cosmeticItem.AmountPurchased += amountPurchased;
                cosmeticItem.AmountAvailable += amountAvailable;
                Save();
                return;
            }
        }

        Cosmetics.Add(new CosmeticItemModel { Name = name, AmountPurchased = amountPurchased, AmountAvailable = amountAvailable });
        Save();
    }

    public void UpdateCurrency(string name, int amount)
    {
        foreach(InGameCurrency currency in Currencies)
        {
            if(currency.Name == name)
            {
                currency.Amount += amount;
            }
        }

        Save();
    }

    public void UpdateAliensRescued(int alienId, int amount)
    {
        foreach(AlienModel alien in AliensRescued)
        {
            if(alien.Id == alienId)
            {
                alien.Amount += amount;
                Save();
                return;
            }
        }

        AliensRescued.Add(new AlienModel { Id = alienId, Amount = amount });
        Save();
    }

    public void Save()
    {
        progressionProvider.Save(JsonUtility.ToJson(this));
    }

    public void Load(GameConfigService config)
    {
        string data = progressionProvider.Load();
        Debug.Log("2");
        if (string.IsNullOrEmpty(data))
        {
            Currencies = config.InitialCurrencies;
            ActiveBoosters = config.InitialActiveBoosters;
            CurrentLevel = 1;
            HudColors = new List<ColorHudItemModel>();
            HudColors.Add(new ColorHudItemModel { Name = "Orange", Selected = true });
            currentHudColor = "Orange";
            Cosmetics = new List<CosmeticItemModel>();
            Debug.Log("CC");
            Save();
        }
        else
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }
        
    }
//end of save and load

    public int GetCurrency(string name)
    {
        foreach(InGameCurrency curreny in Currencies)
        {
            if (curreny.Name == name)
                return curreny.Amount;
        }

        return 0;
    }

    public int GetCosmeticAmount(string name)
    {
        foreach(CosmeticItemModel cosmetic in Cosmetics)
        {
            if(cosmetic.Name == name)
            {
                return cosmetic.AmountPurchased;
            }
        }

        return 0;
    }

    public int GetActiveBoosterAmount(string name)
    {
        foreach (ActiveBoosterItemModel booster in ActiveBoosters)
        {
            if (booster.Name == name)
            {
                return booster.Amount;
            }
        }

        return 0;
    }

    public bool IsHudColorActive(string name)
    {
        /*foreach(ColorHudItemModel colorHud in HudColors)
        {
            if(colorHud.Name == name)
            {
                return colorHud.Selected;
            }
        }

        return false;*/
        return name == currentHudColor;
    }

    public void Clear()
    {
    }
}