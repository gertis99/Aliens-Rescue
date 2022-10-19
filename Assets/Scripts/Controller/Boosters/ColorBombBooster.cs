using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBombBooster : ABooster
{
    private int alienType;

    public override void Execute(Vector2 pos, Grid model)
    {
        gridModel = model;

        List<Element> elements = new List<Element>();

        alienType = ((Alien)gridModel.GridLevel[(int)pos.x, (int)pos.y]).AlienId;

        for(int i = 0; i < gridModel.GridLevel.GetLength(0); i++)
        {
            for(int j = 0; j < gridModel.GridLevel.GetLength(1); j++)
            {
                if (gridModel.GridLevel[i, j] != null && ((Alien)gridModel.GridLevel[i, j]).AlienId == alienType)
                {
                    elements.Add(gridModel.GridLevel[i, j]);
                    levelController.DestroyCell(i, j, false);
                }
            }
        }

        levelController.AddPoints(elements);
    }
}
