using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBoosterController
{
    public delegate void ActiveBooster(IBooster booster, Vector2 pos);
    public static event ActiveBooster OnBoosterActived;
    private IBooster actualBooster;

    public void ActiveBoosterLineVertical(Vector2 pos)
    {
        actualBooster = new VerticalLineBooster();
        OnBoosterActived(actualBooster, pos);
        actualBooster = null;
    }

    public void ActiveBoosterLineHorizontal(Vector2 pos)
    {
        actualBooster = new HorizontalLineBooster();
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
