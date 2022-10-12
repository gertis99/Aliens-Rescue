using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLineBooster : ABooster
{
    public override void Execute(Vector2 pos, Grid model)
    {
        gridModel = model;

        List<Element> elements = new List<Element>();

        for (int i = 0; i < gridModel.GridLevel.GetLength(0); i++)
        {
            if (gridModel.GridLevel[i, (int)pos.y] != null)
            {
                elements.Add(gridModel.GridLevel[i, (int)pos.y]);
                levelController.DestroyCell(i, (int)pos.y, false);
            }
        }

        levelController.AddPoints(elements);
    }
}
