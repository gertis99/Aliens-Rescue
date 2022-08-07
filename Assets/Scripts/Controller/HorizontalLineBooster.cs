using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLineBooster : IBooster
{
    public void Execute(Vector2 pos, ref Element[,] gridLevel)
    {
        List<Element> elements = new List<Element>();

        for (int i = 0; i < gridLevel.GetLength(0); i++)
        {
            if (gridLevel[i, (int)pos.y] != null)
            {
                elements.Add(gridLevel[i, (int)pos.y]);
                gridLevel[i, (int)pos.y] = null;
            }
        }

        WinController.AddPoints(elements);
    }
}
