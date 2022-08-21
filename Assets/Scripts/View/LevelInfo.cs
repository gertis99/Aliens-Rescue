using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public ActiveBoosterHorizontalLineView horizotntalBooster;
    public ActiveBoosterVerticalLineView verticalBooster;
    public ActiveBoosterBombView bombBooster;
    public ActiveBoosterColorBombView colorBombBooster;


    void Awake()
    {
        horizotntalBooster.boostersLeft = PlayerInfo.HorizontalBoosters;
        verticalBooster.boostersLeft = PlayerInfo.VerticalBoosters;
        bombBooster.boostersLeft = PlayerInfo.BombBoosters;
        colorBombBooster.boostersLeft = PlayerInfo.ColorBombBoosters;
    }

}
