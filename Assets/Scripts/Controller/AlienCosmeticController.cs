using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCosmeticController
{
    private GameProgressionService gameProgression;
    private GameConfigService gameConfig;

    public AlienCosmeticController()
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    public void SelectedCosmetic(string cosmeticName, int alienId)
    {
        // Unlock the other one
        foreach(CosmeticItemModel cosmetic in gameProgression.Cosmetics)
        {
            if (cosmetic.SelectedAliensIds.Contains(alienId) && cosmetic.Name != cosmeticName)
            {
                cosmetic.AmountAvailable++;
                cosmetic.SelectedAliensIds.Remove(alienId);
                break;
            }
        }

        // Lock/Unlock the new one
        foreach (CosmeticItemModel cosmetic in gameProgression.Cosmetics)
        {
            if (cosmetic.SelectedAliensIds.Contains(alienId) && cosmetic.Name == cosmeticName)
            {
                cosmetic.AmountAvailable++;
                cosmetic.SelectedAliensIds.Remove(alienId);
                break;
            }

            if(cosmetic.Name == cosmeticName)
            {
                cosmetic.AmountAvailable--;
                cosmetic.SelectedAliensIds.Add(alienId);
            }
        }
    }
}
