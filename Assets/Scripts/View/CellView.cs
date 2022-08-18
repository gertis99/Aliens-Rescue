using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour
{
    public Vector2Int position;
    public int colorType;

    public void Initialize(Vector2Int pos, int color)
    {
        position = pos;
        colorType = color;
    }
}
