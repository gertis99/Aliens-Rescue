using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBombBooster : ABooster
{
    private AlienType alienType;

    public override bool CheckCondition(int nHorizontal, int nVertical)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(Vector2 pos, Grid model)
    {
        gridModel = model;

        List<Element> elements = new List<Element>();

        alienType = ((Alien)gridModel.GridLevel[(int)pos.x, (int)pos.y]).GetElementType();

        for(int i = 0; i < gridModel.GridLevel.GetLength(0); i++)
        {
            for(int j = 0; j < gridModel.GridLevel.GetLength(1); j++)
            {
                if (((Alien)gridModel.GridLevel[i, j]).GetElementType() == alienType)
                {
                    elements.Add(gridModel.GridLevel[i, j]);
                    LevelController.levelControllerInstance.DestroyCell(i, j, false);
                }
            }
        }

        LevelController.levelControllerInstance.AddPoints(elements);
    }
}
