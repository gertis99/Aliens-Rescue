using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class which generate and store the base grid of the game

public class Grid : MonoBehaviour
{

    public GameObject tile;
    public int width, height;
    private GameObject[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[width, height];
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateGrid()
    {
        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                grid[i, j] = Instantiate(tile, new Vector2(i, j), Quaternion.identity);
                grid[i, j].transform.SetParent(this.transform, true);
            }
        }
    }

    public GameObject[,] GetGrid()
    {
        return grid;
    }
}
