using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBooster : IBooster
{
    public void Execute(Vector2 pos, ref Element[,] gridLevel)
    {
        List<Element> elements = new List<Element>();


        elements.Add(gridLevel[(int)pos.x, (int)pos.y]);
        gridLevel[(int)pos.x, (int)pos.y] = null;

        if (LevelController.IsOnLevel((int)pos.x + 1, (int)pos.y)) { elements.Add(gridLevel[(int)pos.x + 1, (int)pos.y]); gridLevel[(int)pos.x + 1, (int)pos.y] = null; }
        if (LevelController.IsOnLevel((int)pos.x - 1, (int)pos.y)) { elements.Add(gridLevel[(int)pos.x - 1, (int)pos.y]); gridLevel[(int)pos.x - 1, (int)pos.y] = null; }
        if (LevelController.IsOnLevel((int)pos.x + 1, (int)pos.y + 1)){ elements.Add(gridLevel[(int)pos.x + 1, (int)pos.y + 1]); gridLevel[(int)pos.x + 1, (int)pos.y + 1] = null; }
        if (LevelController.IsOnLevel((int) pos.x + 1, (int) pos.y - 1)) { elements.Add(gridLevel[(int)pos.x + 1, (int)pos.y - 1]); gridLevel[(int)pos.x + 1, (int)pos.y - 1] = null; }
        if (LevelController.IsOnLevel((int)pos.x - 1, (int)pos.y + 1)) { elements.Add(gridLevel[(int)pos.x - 1, (int)pos.y + 1]); gridLevel[(int)pos.x-1, (int)pos.y+1] = null; }
        if (LevelController.IsOnLevel((int)pos.x - 1, (int)pos.y - 1)) { elements.Add(gridLevel[(int)pos.x - 1, (int)pos.y - 1]); gridLevel[(int)pos.x-1, (int)pos.y-1] = null; }
        if (LevelController.IsOnLevel((int)pos.x, (int)pos.y + 1)) { elements.Add(gridLevel[(int)pos.x, (int)pos.y + 1]); gridLevel[(int)pos.x, (int)pos.y+1] = null; }
        if (LevelController.IsOnLevel((int)pos.x, (int)pos.y - 1)) { elements.Add(gridLevel[(int)pos.x, (int)pos.y - 1]); gridLevel[(int)pos.x, (int)pos.y-1] = null; }

        WinController.AddPoints(elements);
    }
}
