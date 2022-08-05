using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBooster
{
    
    void Execute(Vector2 pos, ref Element[,] gridLevel);
}
