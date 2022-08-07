using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Show the grid to the player
 */
public class GridView : MonoBehaviour
{

    public GameObject[] levelElemnts;
    public GameObject[] baseElement;
    public Sprite[] sprites;
    private GameObject[,] gridLevel;

    private void Awake()
    {
        LevelController.OnGridCreated += CreateLevel;
        LevelController.OnGridChanged += UpdateLevel;
    }

    private void OnDisable()
    {
        LevelController.OnGridCreated -= CreateLevel;
        LevelController.OnGridChanged -= UpdateLevel;
    }

    private void UpdateLevel(Element[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if(grid[i,j] == null)
                {
                    gridLevel[i, j] = Instantiate(levelElemnts[grid[i, j].GetColorType()], new Vector2(i, j), Quaternion.identity, this.transform);
                }
                else
                {
                    /*if(grid[i, j].GetColorType() < 5)
                        gridLevel[i, j].GetComponent<SpriteRenderer>().sprite = sprites[grid[i,j].GetColorType()];
                    else
                        gridLevel[i, j] = Instantiate(levelElemnts[grid[i, j].GetColorType()], new Vector2(i, j), Quaternion.identity, this.transform);*/
                    Destroy(gridLevel[i, j]);
                    gridLevel[i, j] = Instantiate(levelElemnts[grid[i, j].GetColorType()], new Vector2(i, j), Quaternion.identity, this.transform);
                }
            }
        }
    }

    

    private void CreateLevel(Element[,] grid)
    {
        gridLevel = new GameObject[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                gridLevel[i,j] = Instantiate(levelElemnts[grid[i,j].GetColorType()], new Vector2(i, j), Quaternion.identity);
                gridLevel[i, j].transform.SetParent(this.transform, true);
            }
        }
    }
}
