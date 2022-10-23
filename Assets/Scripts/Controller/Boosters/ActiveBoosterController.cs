using System;
using UnityEngine;

public class ActiveBoosterController
{
    public static Action<ABooster, Vector2> OnBoosterActived;
    private ABooster currentBooster;

    public void ActiveBoosterLineVertical(Vector2 pos)
    {
        currentBooster = new VerticalLineBooster();
        OnBoosterActived(currentBooster, pos);
        currentBooster = null;
    }

    public void ActiveBoosterLineHorizontal(Vector2 pos)
    {
        currentBooster = new HorizontalLineBooster();
        OnBoosterActived(currentBooster, pos);
        currentBooster = null;
    }

    public void ActiveBoosterBomb(Vector2 pos)
    {
        currentBooster = new BombBooster();
        OnBoosterActived(currentBooster, pos);
        currentBooster = null;
    }

    public void ActiveBoosterColorBomb(Vector2 pos)
    {
        currentBooster = new ColorBombBooster();
        OnBoosterActived(currentBooster, pos);
        currentBooster = null;
    }
}
