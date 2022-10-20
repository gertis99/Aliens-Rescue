using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameProgressionProvider : IGameProgressionProvider
{
    private FileGameProgressionProvider _local = new FileGameProgressionProvider();
    private RemoteGameProgressionProvider _remote = new RemoteGameProgressionProvider();

    public async Task<bool> Initialize()
    {
        await Task.WhenAll(_local.Initialize(), _remote.Initialize());
        return true;
    }

    public void Save(string data)
    {
        _local.Save(data);
        _remote.Save(data);
    }

    public string Load()
    {
        Debug.Log("AA");
        string localData = _local.Load();
        Debug.Log("BB");
        string remoteData = _remote.Load();
        Debug.Log("CC");
        /*if (string.IsNullOrEmpty(localData) && !string.IsNullOrEmpty(remoteData))
        {
            return remoteData;
        }*/
        Debug.Log("DD");
        if (!string.IsNullOrEmpty(localData) && string.IsNullOrEmpty(remoteData))
        {
            return localData;
        }
        Debug.Log("EE");
        SaveComparator fileSave = new SaveComparator();
        SaveComparator remoteSave = new SaveComparator();
        JsonUtility.FromJsonOverwrite(localData, fileSave);
        JsonUtility.FromJsonOverwrite(remoteData, remoteSave);
        Debug.Log("FF");
        //decide which one to keep
        // Cosmetics
        int cosmeticsFile = 0, cosmeticsRemote = 0;
        foreach(CosmeticItemModel cosmeticFile in fileSave.Cosmetics) {
            cosmeticsFile += cosmeticFile.AmountPurchased;
        }

        foreach (CosmeticItemModel cosmeticRemote in remoteSave.Cosmetics)
        {
            cosmeticsRemote += cosmeticRemote.AmountPurchased;
        }

        if (cosmeticsRemote > cosmeticsFile)
            return remoteData;

        if (cosmeticsFile > cosmeticsRemote)
            return localData;

        // Level
        if (fileSave.CurrentLevel > remoteSave.CurrentLevel)
            return localData;

        if (remoteSave.CurrentLevel > fileSave.CurrentLevel)
            return remoteData;

        // Boosters
        int boostersFile = 0, boostersRemote = 0;
        foreach (ActiveBoosterItemModel boosterModel in fileSave.ActiveBoosters)
        {
            boostersFile += boosterModel.Amount;
        }

        foreach (ActiveBoosterItemModel boosterModel in remoteSave.ActiveBoosters)
        {
            boostersRemote += boosterModel.Amount;
        }

        if (boostersRemote > boostersFile)
            return remoteData;

        if (boostersFile > boostersRemote)
            return localData;

        // Currencies
        if (fileSave.Currencies[0].Amount > remoteSave.Currencies[0].Amount)
            return localData;

        if (fileSave.Currencies[0].Amount < remoteSave.Currencies[0].Amount)
            return remoteData;
        Debug.Log("GG");
        // If it is the same
        return localData;
    }
}