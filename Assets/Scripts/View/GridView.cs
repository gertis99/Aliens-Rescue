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
    private List<IAnimation> animations = new List<IAnimation>();
    private bool IsAnimating => animations.Count > 0;

    private void Awake()
    {
        
        levelController = new LevelController(width, height, colorTypes);
        LevelController.OnGridCreated += CreateLevel;
        //LevelController.OnGridChanged += UpdateLevel;
        levelController.OnCellCreated += CreateCellView;
        levelController.OnCellMoved += MoveCellView;
        levelController.OnCellDestroyed += DestroyCellView;
        levelController.OnCellsSwapped += SwapCellsView;
    }

    private void Start()
    {
        levelController.CreateGrid();
    }

    private void OnDisable()
    {
        LevelController.OnGridCreated -= CreateLevel;
        //LevelController.OnGridChanged -= UpdateLevel;
        levelController.OnCellCreated -= CreateCellView;
        levelController.OnCellMoved -= MoveCellView;
        levelController.OnCellDestroyed -= DestroyCellView;
        levelController.OnCellsSwapped -= SwapCellsView;
    }

    private void Update()
    {
        if (IsAnimating)
            return;

        CheckInputs();
    }

    private void DestroyCellView(Element el)
    {
        if (gridLevel[el.GetPosX(), el.GetPosY()] != null)
        {
            animations.Add(new DestroyCellAnimation(gridLevel[el.GetPosX(), el.GetPosY()]));
            if (animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
        }
    }

    public void DestroyCell(GameObject obj)
    {
        Destroy(obj);
    }

    private void MoveCellView(Element el, Vector2Int pos)
    {
        if(gridLevel[el.GetPosX(), el.GetPosY()] != null)
        {
            animations.Add(new MoveCellAnimation(new Vector2Int(pos.x, pos.y), gridLevel[el.GetPosX(), el.GetPosY()]));
            if (animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
            gridLevel[pos.x, pos.y] = gridLevel[el.GetPosX(), el.GetPosY()];
        }
        
    }

    private void SwapCellsView(Element el1, Element el2)
    {
        animations.Add(new SwapCellsAnimation(gridLevel[el1.GetPosX(), el1.GetPosY()], gridLevel[el2.GetPosX(), el2.GetPosY()]));
        if (animations.Count == 1)
        {
            StartCoroutine(ProcessAnimations());
        }
        
        GameObject aux = gridLevel[el1.GetPosX(), el1.GetPosY()];
        gridLevel[el1.GetPosX(), el1.GetPosY()] = gridLevel[el2.GetPosX(), el2.GetPosY()];
        gridLevel[el2.GetPosX(), el2.GetPosY()] = aux;
    }

    private void CreateCellView(Element el)
    {
        if(el.GetColorType() < 6)
            gridLevel[el.GetPosX(), el.GetPosY()] = Instantiate(levelElemnts[el.GetColorType()], new Vector2(el.GetPosX(), gridLevel.GetLength(1)+10), Quaternion.identity, this.transform);
        else
            gridLevel[el.GetPosX(), el.GetPosY()] = Instantiate(levelElemnts[el.GetColorType()], new Vector2(el.GetPosX(), el.GetPosY()), Quaternion.identity, this.transform);

        gridLevel[el.GetPosX(), el.GetPosY()].SetActive(false);


        animations.Add(new CreateCellAnimation(new Vector2Int(el.GetPosX(), el.GetPosY()), gridLevel[el.GetPosX(), el.GetPosY()]));
        if (animations.Count == 1)
        {
            StartCoroutine(ProcessAnimations());
        }
    }

    private IEnumerator ProcessAnimations()
    {
        while (IsAnimating)
        {
            yield return animations[0].PlayAnimation(this);
            animations.RemoveAt(0);
        }
    }

    private void CheckInputs()
    {
        if (Input.GetMouseButtonDown(0))
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
        }
        

        if (elementSelected != null && Input.GetMouseButton(0) && !IsAnimating)
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
        else
        {
            elementSelected = null;
        }
    }

    /*private void UpdateLevel(Element[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if(grid[i,j] == null)
                {
                    //gridLevel[i, j] = Instantiate(levelElemnts[grid[i, j].GetColorType()], new Vector2(i, j), Quaternion.identity, this.transform);
                }
                else
                {
                    /*if(grid[i, j].GetColorType() < 5)
                        gridLevel[i, j].GetComponent<SpriteRenderer>().sprite = sprites[grid[i,j].GetColorType()];
                    else
                        gridLevel[i, j] = Instantiate(levelElemnts[grid[i, j].GetColorType()], new Vector2(i, j), Quaternion.identity, this.transform);*/
                    /*Destroy(gridLevel[i, j]);
                    gridLevel[i, j] = Instantiate(levelElemnts[grid[i, j].GetColorType()], new Vector2(i, j), Quaternion.identity, this.transform);
                    gridLevel[i, j].GetComponent<CellView>().Initialize(new Vector2Int(i, j), grid[i, j].GetColorType());
                }
            }
        }
    }*/

    

    private void CreateLevel(Element[,] grid)
    {
        gridLevel = new GameObject[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                gridLevel[i,j] = Instantiate(levelElemnts[grid[i,j].GetColorType()], new Vector2(i, j), Quaternion.identity, this.transform);
                gridLevel[i, j].GetComponent<CellView>().Initialize(new Vector2Int(i, j), grid[i, j].GetColorType());
            }
        }
    }
}
