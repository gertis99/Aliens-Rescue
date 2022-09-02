using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLineBooster : ABooster
{
    public override bool CheckCondition(int nHorizontal, int nVertical)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(Vector2 pos, Grid model)
    {
        gridModel = model;

        List<Element> elements = new List<Element>();

        for (int i = 0; i < gridModel.GridLevel.GetLength(0); i++)
        {
            if (gridModel.GridLevel[i, (int)pos.y] != null)
            {
                elements.Add(gridModel.GridLevel[i, (int)pos.y]);
                LevelController.levelControllerInstance.DestroyCell(i, (int)pos.y, false);
            }
        }

        LevelController.levelControllerInstance.AddPoints(elements);
    }
}
