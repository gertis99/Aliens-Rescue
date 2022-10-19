using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class containing the info about the grid of each level
 * */

public class Grid
{

    public Element[,] GridLevel { get; set; }
    public int width = 9, height = 9;
    public int colorTypes = 6;

    public Grid(int w, int h, int colors)
    {
        width = w;
        height = h;
        colorTypes = colors;
        GridLevel = new Element[width, height];
        GenerateLevel();
    }

    /*private void Awake()
    {
        GenerateLevel();
    }*/

    private void GenerateLevel()
    {
        

        for (int i = 0; i < GridLevel.GetLength(0); i++)
        {
            for (int j = 0; j < GridLevel.GetLength(1); j++)
            {
                GridLevel[i, j] = new Alien(i, j, UnityEngine.Random.Range(0, colorTypes));
            }
        }
    }

    /**************************************************************************/
    // Getters
    public Element[,] GetGridLevel()
    {
        return GridLevel;
    }

    public Element GetElement(int x, int y)
    {
        return GridLevel[x, y];
    }

    /*************************************************************************/
    // Setters
    public void SetGridLevel(Element[,] grid)
    {
        GridLevel = grid;
    }

    /**************************************************************************/
    public bool IsOnGrid(int row, int col)
    {
        if (row < GridLevel.GetLength(0) && col < GridLevel.GetLength(1) && row >= 0 && col >= 0 && GridLevel[row, col] != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
