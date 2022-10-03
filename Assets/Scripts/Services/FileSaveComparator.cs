using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileSaveComparator
{
    public int CurrentLevel;
    public List<ActiveBoosterItemModel> ActiveBoosters;
    public List<CosmeticItemModel> Cosmetics;
    public List<ColorHudItemModel> HudColors;
    public string currentHudColor;
    public List<InGameCurrency> Currencies;
}
