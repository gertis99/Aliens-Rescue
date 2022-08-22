using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadGame
{
    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.dataPath + "/gameData.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataToSave data = new DataToSave();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadGame()
    {
        string path = Application.dataPath + "/gameData.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataToSave data = formatter.Deserialize(stream) as DataToSave;

            stream.Close();

            PlayerInfo.Coins = data.coins;
            PlayerInfo.HorizontalBoosters = data.horizontalBoosters;
            PlayerInfo.VerticalBoosters = data.verticalBoosters;
            PlayerInfo.BombBoosters = data.bombBoosters;
            PlayerInfo.ColorBombBoosters = data.colorBombBoosters;
            PlayerInfo.ActualLevel = data.actualLevel;
        }
        else
        {
            PlayerInfo.Coins = 100;
            PlayerInfo.HorizontalBoosters = 5;
            PlayerInfo.VerticalBoosters = 5;
            PlayerInfo.BombBoosters = 5;
            PlayerInfo.ColorBombBoosters = 5;
            PlayerInfo.ActualLevel = 1;
        }
    }
}
