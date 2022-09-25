using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : Element
{
    private BoosterType boosterType;

    public Booster(int x, int y, BoosterType bType)
    {
        posX = x;
        posY = y;
        boosterType = bType;
    }

    public BoosterType GetElementType()
    {
        return boosterType;
    }
}
