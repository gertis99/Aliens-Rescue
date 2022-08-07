using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBombBooster : IBooster
{
    private int elementColor;

    public void Execute(Vector2 pos, ref Element[,] gridLevel)
    {
        List<Element> elements = new List<Element>();

        elementColor = gridLevel[(int)pos.x, (int)pos.y].GetColorType();

        for(int i = 0; i < gridLevel.GetLength(0); i++)
        {
            for(int j = 0; j < gridLevel.GetLength(1); j++)
            {
                if (gridLevel[i, j].GetColorType() == elementColor)
                {
                    elements.Add(gridLevel[i, j]);
                    gridLevel[i, j] = null;
                }
            }
        }

        WinController.AddPoints(elements);
    }
}
