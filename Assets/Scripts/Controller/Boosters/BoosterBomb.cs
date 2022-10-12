using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBooster : ABooster
{
    public BombBooster()
    {

    }

    public override void Execute(Vector2 pos, Grid model)
    {
        gridModel = model;

        List<Element> elements = new List<Element>();


        elements.Add(gridModel.GridLevel[(int)pos.x, (int)pos.y]);
        levelController.DestroyCell((int)pos.x, (int)pos.y, false);

        if (gridModel.IsOnGrid((int)pos.x + 1, (int)pos.y)) { elements.Add(gridModel.GridLevel[(int)pos.x + 1, (int)pos.y]); levelController.DestroyCell((int)pos.x + 1, (int)pos.y, false); }
        if (gridModel.IsOnGrid((int)pos.x - 1, (int)pos.y)) { elements.Add(gridModel.GridLevel[(int)pos.x - 1, (int)pos.y]); levelController.DestroyCell((int)pos.x - 1, (int)pos.y, false); }
        if (gridModel.IsOnGrid((int)pos.x + 1, (int)pos.y + 1)){ elements.Add(gridModel.GridLevel[(int)pos.x + 1, (int)pos.y + 1]); levelController.DestroyCell((int)pos.x + 1, (int)pos.y + 1, false); }
        if (gridModel.IsOnGrid((int) pos.x + 1, (int) pos.y - 1)) { elements.Add(gridModel.GridLevel[(int)pos.x + 1, (int)pos.y - 1]); levelController.DestroyCell((int)pos.x + 1, (int)pos.y - 1, false); }
        if (gridModel.IsOnGrid((int)pos.x - 1, (int)pos.y + 1)) { elements.Add(gridModel.GridLevel[(int)pos.x - 1, (int)pos.y + 1]); levelController.DestroyCell((int)pos.x - 1, (int)pos.y + 1, false); }
        if (gridModel.IsOnGrid((int)pos.x - 1, (int)pos.y - 1)) { elements.Add(gridModel.GridLevel[(int)pos.x - 1, (int)pos.y - 1]); levelController.DestroyCell((int)pos.x - 1, (int)pos.y - 1, false); }
        if (gridModel.IsOnGrid((int)pos.x, (int)pos.y + 1)) { elements.Add(gridModel.GridLevel[(int)pos.x, (int)pos.y + 1]); levelController.DestroyCell((int)pos.x, (int)pos.y + 1, false); }
        if (gridModel.IsOnGrid((int)pos.x, (int)pos.y - 1)) { elements.Add(gridModel.GridLevel[(int)pos.x, (int)pos.y - 1]); levelController.DestroyCell((int)pos.x, (int)pos.y - 1, false); }

        levelController.AddPoints(elements);
    }

}
