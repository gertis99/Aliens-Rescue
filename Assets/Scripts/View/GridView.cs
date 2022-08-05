using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Show the grid to the player
 */
public class GridView : MonoBehaviour
{
    public int width = 9, height = 9, colorTypes = 6;

    public GameObject[] levelElemnts;
    public GameObject[] baseElement;
    public Sprite[] sprites;
    private GameObject[,] gridLevel;
    private LevelController levelController;
    private GameObject elementSelected = null;

    private void Awake()
    {
        
        levelController = new LevelController(width, height, colorTypes);
        LevelController.OnGridCreated += CreateLevel;
        LevelController.OnGridChanged += UpdateLevel;
    }

    private void Start()
    {
        levelController.CreateGrid();
    }

    private void OnDisable()
    {
        LevelController.OnGridCreated -= CreateLevel;
        LevelController.OnGridChanged -= UpdateLevel;
    }

    private void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (Input.GetMouseButton(0))
        {
            if (elementSelected == null)
            {
                RaycastHit2D hitData = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);
                if (hitData)
                {
                    elementSelected = hitData.collider.gameObject;
                    levelController.SetElementSelected(elementSelected);
                }
            }
            else
            {
                if (elementSelected.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x <= -0.5) // Derecha
                {
                    //Debug.Log("Derecha");
                    levelController.CheckTryToMove(elementSelected.transform.position);
                    //OnElementMoved(elementSelected.transform.position);
                    elementSelected = null;
                }
                else if (elementSelected.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= 0.5)  // Izquierda
                {
                    //Debug.Log("Izquierda");
                    levelController.CheckTryToMove(elementSelected.transform.position);
                    elementSelected = null;
                }
                else if (elementSelected.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= -0.5) // Arriba
                {
                    //Debug.Log("Arriba");
                    levelController.CheckTryToMove(elementSelected.transform.position);
                    elementSelected = null;
                }
                else if (elementSelected.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y >= 0.5)  // Abajo
                {
                    //Debug.Log("Abajo");
                    levelController.CheckTryToMove(elementSelected.transform.position);
                    elementSelected = null;
                }
            }
        }
        else
        {
            elementSelected = null;
        }
    }

    private void UpdateLevel(Element[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if(grid[i,j] == null)
                {
                    gridLevel[i, j] = Instantiate(levelElemnts[grid[i, j].GetColorType()], new Vector2(i, j), Quaternion.identity);
                    gridLevel[i, j].transform.SetParent(this.transform, true);
                }
                else
                {
                    gridLevel[i, j].GetComponent<SpriteRenderer>().sprite = sprites[grid[i,j].GetColorType()];
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
