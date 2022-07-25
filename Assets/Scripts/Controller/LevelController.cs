using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manage the movement of the elements in the level
 * */

public class LevelController : MonoBehaviour
{
    public delegate void GridCreated(Element[,] grid);
    public static event GridCreated OnGridCreated;
    public static event GridCreated OnGridChanged;

    public int width = 9, height = 9, colorTypes = 6;
    private Grid gridObject;
    private Element[,] gridLevel;

    // Start is called before the first frame update
    void Start()
    {
        gridObject = new Grid(width, height, colorTypes);
        gridLevel = gridObject.GetGridLevel();
        OnGridCreated(gridLevel);
    }

    private void OnEnable()
    {
        SwapsController.OnSwapDone += MoveDownPieces;
    }

    private void OnDisable()
    {
        SwapsController.OnSwapDone -= MoveDownPieces;
    }

    public void OnLevelChange()
    {

    }

    // Move all the pieces down if it is possible
    private void MoveDownPieces(Element[,] grid)
    {
        gridLevel = grid;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gridLevel[x, y] == null)
                {
                    for (int yAux = y + 1; yAux < height; yAux++)
                    {
                        if (gridLevel[x, yAux] != null)
                        {
                            gridLevel[x, y] = gridLevel[x, yAux];
                            gridLevel[x, yAux] = null;
                            gridLevel[x, y].SetPos(x,y);
                            break;
                        }
                    }
                }
            }
        }

        gridObject.SetGridLevel(gridLevel);
        FillBlanks();
    }

    private void FillBlanks()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gridLevel[x, y] == null)
                {
                    gridLevel[x, y] = new Element(x, y, UnityEngine.Random.Range(0, colorTypes-1));
                }
            }
        }

        gridObject.SetGridLevel(gridLevel);
        OnGridChanged(gridLevel);
    }
}
