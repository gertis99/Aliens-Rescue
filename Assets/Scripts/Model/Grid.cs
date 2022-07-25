using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class containing the info about the grid of each level
 * */

public class Grid
{

    private Element[,] gridLevel;
    public int width = 9, height = 9;
    public int colorTypes = 6;

    public Grid(int w, int h, int colors)
    {
        width = w;
        height = h;
        colorTypes = colors;
        gridLevel = new Element[width, height];
        GenerateLevel();
    }

    /*private void Awake()
    {
        GenerateLevel();
    }*/

    private void GenerateLevel()
    {
        for (int i = 0; i < gridLevel.GetLength(0); i++)
        {
            for (int j = 0; j < gridLevel.GetLength(1); j++)
            {
                gridLevel[i, j] = new Element(i, j, UnityEngine.Random.Range(0, colorTypes-1));
            }
        }
    }

    /**************************************************************************/
    // Getters
    public Element[,] GetGridLevel()
    {
        return gridLevel;
    }

    public Element GetElement(int x, int y)
    {
        return gridLevel[x, y];
    }

    /*************************************************************************/
    // Setters
    public void SetGridLevel(Element[,] grid)
    {
        gridLevel = grid;
    }
}
