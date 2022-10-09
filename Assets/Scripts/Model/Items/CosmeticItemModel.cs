using System.Collections.Generic;

[System.Serializable]
public class CosmeticItemModel
{
    public string Name;
    public int AmountPurchased;
    public int AmountAvailable;
    public List<int> SelectedAliensIds = new List<int>();
}
