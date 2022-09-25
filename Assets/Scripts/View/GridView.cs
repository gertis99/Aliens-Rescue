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

    public GameObject blueAlienPrefab, redAlienPrefab, yellowAlienPrefab, purpleAlienPrefab, greenAlienPrefab, orangeAlienPrefab;
    public GameObject horizontalLineBoosterPrefab, verticalLineBoosterPrefab, bombPrefab, colorBombPrefab;
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
        if (el is Alien)
            InstantiateElement(el, el.GetPosX(), el.GetPosY(), new Vector2(el.GetPosX(), gridLevel.GetLength(1) + 10));
        //gridLevel[el.GetPosX(), el.GetPosY()] = Instantiate(alienElemnts[(int)((Alien)el).GetElementType()], new Vector2(el.GetPosX(), gridLevel.GetLength(1)+10), Quaternion.identity, this.transform);
        else
            InstantiateElement(el, el.GetPosX(), el.GetPosY(), new Vector2(el.GetPosX(), el.GetPosY()));
            //gridLevel[el.GetPosX(), el.GetPosY()] = Instantiate(boosterElemnts[(int)((Booster)el).GetElementType()], new Vector2(el.GetPosX(), el.GetPosY()), Quaternion.identity, this.transform);

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

    private void CreateLevel(Element[,] grid)
    {
        gridLevel = new GameObject[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                InstantiateElement(grid[i, j], i, j, new Vector2(i, j));
                //gridLevel[i,j] = Instantiate(alienElemnts[(int)((Alien)grid[i,j]).GetElementType()], new Vector2(i, j), Quaternion.identity, this.transform);
                gridLevel[i, j].GetComponent<CellView>().Initialize(new Vector2Int(i, j), (int)((Alien)grid[i, j]).GetElementType());
            }
        }
    }

    private void InstantiateElement(Element element, int row, int col, Vector2 pos)
    {
        if(element is Alien)
        {
            switch (((Alien)element).GetElementType())
            {
                case AlienType.BlueAlien:
                    gridLevel[row, col] = Instantiate(blueAlienPrefab, pos, Quaternion.identity, this.transform);
                    break;
                case AlienType.RedAlien:
                    gridLevel[row, col] = Instantiate(redAlienPrefab, pos, Quaternion.identity, this.transform);
                    break;
                case AlienType.GreenAlien:
                    gridLevel[row, col] = Instantiate(greenAlienPrefab, pos, Quaternion.identity, this.transform);
                    break;
                case AlienType.PurpleAlien:
                    gridLevel[row, col] = Instantiate(purpleAlienPrefab, pos, Quaternion.identity, this.transform);
                    break;
                case AlienType.YellowAlien:
                    gridLevel[row, col] = Instantiate(yellowAlienPrefab, pos, Quaternion.identity, this.transform);
                    break;
                case AlienType.OrangeAlien:
                    gridLevel[row, col] = Instantiate(orangeAlienPrefab, pos, Quaternion.identity, this.transform);
                    break;
            }
        }
        else
        {
            switch (((Booster)element).GetElementType())
            {
                case BoosterType.HorizontalLineBooster:
                    gridLevel[row, col] = Instantiate(horizontalLineBoosterPrefab, pos, Quaternion.identity, this.transform);
                    break;
                case BoosterType.VerticalLineBooster:
                    gridLevel[row, col] = Instantiate(verticalLineBoosterPrefab, pos, Quaternion.identity, this.transform);
                    break;
                case BoosterType.BombBooster:
                    gridLevel[row, col] = Instantiate(bombPrefab, pos, Quaternion.identity, this.transform);
                    break;
                case BoosterType.ColorBombBooster:
                    gridLevel[row, col] = Instantiate(colorBombPrefab, pos, Quaternion.identity, this.transform);
                    break;
            }
        }
    }
}
