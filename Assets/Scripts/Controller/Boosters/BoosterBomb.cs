using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBooster : IBooster
{
    public void Execute(Vector2 pos, ref Element[,] gridLevel)
    {
        List<Element> elements = new List<Element>();


        elements.Add(gridLevel[(int)pos.x, (int)pos.y]);
        LevelController.levelControllerInstance.DestroyCell((int)pos.x, (int)pos.y, false);

        if (LevelController.IsOnLevel((int)pos.x + 1, (int)pos.y)) { elements.Add(gridLevel[(int)pos.x + 1, (int)pos.y]); LevelController.levelControllerInstance.DestroyCell((int)pos.x + 1, (int)pos.y, false); }
        if (LevelController.IsOnLevel((int)pos.x - 1, (int)pos.y)) { elements.Add(gridLevel[(int)pos.x - 1, (int)pos.y]); LevelController.levelControllerInstance.DestroyCell((int)pos.x - 1, (int)pos.y, false); }
        if (LevelController.IsOnLevel((int)pos.x + 1, (int)pos.y + 1)){ elements.Add(gridLevel[(int)pos.x + 1, (int)pos.y + 1]); LevelController.levelControllerInstance.DestroyCell((int)pos.x + 1, (int)pos.y + 1, false); }
        if (LevelController.IsOnLevel((int) pos.x + 1, (int) pos.y - 1)) { elements.Add(gridLevel[(int)pos.x + 1, (int)pos.y - 1]); LevelController.levelControllerInstance.DestroyCell((int)pos.x + 1, (int)pos.y - 1, false); }
        if (LevelController.IsOnLevel((int)pos.x - 1, (int)pos.y + 1)) { elements.Add(gridLevel[(int)pos.x - 1, (int)pos.y + 1]); LevelController.levelControllerInstance.DestroyCell((int)pos.x - 1, (int)pos.y + 1, false); }
        if (LevelController.IsOnLevel((int)pos.x - 1, (int)pos.y - 1)) { elements.Add(gridLevel[(int)pos.x - 1, (int)pos.y - 1]); LevelController.levelControllerInstance.DestroyCell((int)pos.x - 1, (int)pos.y - 1, false); }
        if (LevelController.IsOnLevel((int)pos.x, (int)pos.y + 1)) { elements.Add(gridLevel[(int)pos.x, (int)pos.y + 1]); LevelController.levelControllerInstance.DestroyCell((int)pos.x, (int)pos.y + 1, false); }
        if (LevelController.IsOnLevel((int)pos.x, (int)pos.y - 1)) { elements.Add(gridLevel[(int)pos.x, (int)pos.y - 1]); LevelController.levelControllerInstance.DestroyCell((int)pos.x, (int)pos.y - 1, false); }

        LevelController.levelControllerInstance.AddPoints(elements);
    }
}
