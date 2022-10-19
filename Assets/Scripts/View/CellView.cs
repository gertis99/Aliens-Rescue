using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour
{
    [SerializeField] private Vector2Int position;
    [SerializeField] private int colorType;

    public void Initialize(Vector2Int pos, int color)
    {
        position = pos;
        colorType = color;
    }
}
