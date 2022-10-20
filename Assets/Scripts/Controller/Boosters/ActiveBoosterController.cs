using UnityEngine;

public class ActiveBoosterController
{
    public delegate void ActiveBooster(ABooster booster, Vector2 pos);
    public static event ActiveBooster OnBoosterActived;
    private ABooster actualBooster;

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
