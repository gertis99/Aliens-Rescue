using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBoosterController
{
    public delegate void ActiveBooster(IBooster booster, Vector2 pos);
    public static event ActiveBooster OnBoosterActived;
    private IBooster actualBooster;

    public void ActiveBoosterLineCross(Vector2 pos)
    {
        actualBooster = new CrossLineBooster();
        OnBoosterActived(actualBooster, pos);
        actualBooster = null;
    }

    public void ActiveBoosterBomb(Vector2 pos)
    {
        actualBooster = new BombBooster();
        OnBoosterActived(actualBooster, pos);
        actualBooster = null;
    }

    public void ActiveBoosterColorBomb(Vector2 pos)
    {
        actualBooster = new ColorBombBooster();
        OnBoosterActived(actualBooster, pos);
        actualBooster = null;
    }
}
