using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This booster destroy all the horizontal and vertical line
public class CrossLineBooster : IBooster
{
    public void Execute(Vector2 pos, ref Element[,] gridLevel)
    {
        List<Element> elements = new List<Element>();

        for (int i = 0; i < gridLevel.GetLength(0); i++)
        {
            if(gridLevel[i, (int)pos.y] != null)
            {
                elements.Add(gridLevel[i, (int)pos.y]);
                gridLevel[i, (int)pos.y] = null;
            }
        }

        for (int i = 0; i < gridLevel.GetLength(1); i++)
        {
            if (gridLevel[(int)pos.x, i] != null)
            {
                elements.Add(gridLevel[(int)pos.x, i]);
                gridLevel[(int)pos.x, i] = null;
            }
        }

        WinController.AddPoints(elements);
    }
}
