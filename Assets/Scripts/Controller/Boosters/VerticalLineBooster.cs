using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This booster destroy all the horizontal and vertical line
public class VerticalLineBooster : ABooster
{
    public override bool CheckCondition(int nHorizontal, int nVertical)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(Vector2 pos, Grid model)
    {
        gridModel = model;

        List<Element> elements = new List<Element>();

        for (int i = 0; i < gridModel.GridLevel.GetLength(1); i++)
        {
            if (gridModel.GridLevel[(int)pos.x, i] != null)
            {
                elements.Add(gridModel.GridLevel[(int)pos.x, i]);
                LevelController.levelControllerInstance.DestroyCell((int)pos.x, i, false);
            }
        }

        LevelController.levelControllerInstance.AddPoints(elements);
    }
}
