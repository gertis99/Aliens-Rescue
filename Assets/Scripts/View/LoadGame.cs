using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    private void Start()
    {
        PlayerInfo.Coins = 100;
        PlayerInfo.HorizontalBoosters = 5;
        PlayerInfo.VerticalBoosters = 5;
        PlayerInfo.BombBoosters = 5;
        PlayerInfo.ColorBombBoosters = 5;
        PlayerInfo.ActualLevel = 0;
    }
}
