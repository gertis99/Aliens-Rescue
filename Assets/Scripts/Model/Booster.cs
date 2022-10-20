using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : Element
{
    public int BoosterId { get; private set; }

    public Booster(int x, int y, int bType)
    {
        posX = x;
        posY = y;
        BoosterId = bType;
    }
}
