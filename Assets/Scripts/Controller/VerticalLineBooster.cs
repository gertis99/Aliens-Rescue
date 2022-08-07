using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This booster destroy all the horizontal and vertical line
public class VerticalLineBooster : IBooster
{
    public void Execute(Vector2 pos, ref Element[,] gridLevel)
    {
        List<Element> elements = new List<Element>();

        for (int i = 1; i < gridLevel.GetLength(1); i++)
        {
            if (gridLevel[(int)pos.x, i] != null)
            {
                elements.Add(gridLevel[(int)pos.x, i]);
                LevelController.levelControllerInstance.DestroyCell((int)pos.x, i);
            }
        }

        WinController.AddPoints(elements);
    }
}
