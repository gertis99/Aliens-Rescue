using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABooster
{
    protected Grid gridModel;
    //protected Action<int, int, bool> onCellDestroyed;
    protected LevelController levelController;

    public void Initialize(LevelController levelController)
    {
        this.levelController = levelController;
    }

    public abstract void Execute(Vector2 pos, Grid model);
}
