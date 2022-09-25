using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABooster
{
    protected Grid gridModel;

    public abstract bool CheckCondition(int nHorizontal, int nVertical);
    public abstract void Execute(Vector2 pos, Grid model);
}
