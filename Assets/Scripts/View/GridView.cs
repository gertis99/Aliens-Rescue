using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/*
 * Show the grid to the player
 */
public class GridView : MonoBehaviour
{
    [SerializeField]
    private int width = 9, height = 9, colorTypes = 6;
    [SerializeField]
    private GameObject blueAlienPrefab, redAlienPrefab, yellowAlienPrefab, purpleAlienPrefab, greenAlienPrefab, orangeAlienPrefab, genericAlienPrefab;

    public Sprite gold;

    [SerializeField]
    private GameObject horizontalLineBoosterPrefab, verticalLineBoosterPrefab, bombPrefab, colorBombPrefab;
    private GameObject[,] gridLevel;
    private LevelController levelController;
    private GameObject elementSelected = null;
    private List<IAnimation> animations = new List<IAnimation>();
    private bool IsAnimating => animations.Count > 0;
    private GameProgressionService gameProgressionService;
    private GameConfigService gameConfigService;

    private void Awake()
    {
        gameProgressionService = ServiceLocator.GetService<GameProgressionService>();
        gameConfigService = ServiceLocator.GetService<GameConfigService>();
    }

    public void Initialize(LevelController controller)
    {
        levelController = controller;
        levelController.OnGridCreated += CreateLevel;
        levelController.OnCellCreated += CreateCellView;
        levelController.OnCellMoved += MoveCellView;
        levelController.OnCellDestroyed += DestroyCellView;
        levelController.OnCellsSwapped += SwapCellsView;

        levelController.CreateGrid();
    }

    private void OnDisable()
    {
        levelController.OnGridCreated -= CreateLevel;
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
        if (el == null)
            return;

        if (el is Alien)
            InstantiateElement(el, el.GetPosX(), el.GetPosY(), new Vector2(el.GetPosX(), gridLevel.GetLength(1) + 10), ((Alien)el).AlienId);
        //gridLevel[el.GetPosX(), el.GetPosY()] = Instantiate(alienElemnts[(int)((Alien)el).GetElementType()], new Vector2(el.GetPosX(), gridLevel.GetLength(1)+10), Quaternion.identity, this.transform);
        else if(el is Booster)
            InstantiateElement(el, el.GetPosX(), el.GetPosY(), new Vector2(el.GetPosX(), el.GetPosY()), (int)((Booster)el).GetElementType());
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
        

        if (elementSelected != null && Input.GetMouseButton(0))
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
                InstantiateElement(grid[i, j], i, j, new Vector2(i, j), ((Alien)grid[i, j]).AlienId);
                //gridLevel[i,j] = Instantiate(alienElemnts[(int)((Alien)grid[i,j]).GetElementType()], new Vector2(i, j), Quaternion.identity, this.transform);
                //gridLevel[i, j].GetComponent<CellView>().Initialize(new Vector2Int(i, j), ((Alien)grid[i, j]).AlienId);
            }
        }
    }

    private void InstantiateElement(Element element, int row, int col, Vector2 pos, int id)
    {
        // This is going to be implemented without a switch in the near future
        if(element is Alien)
        {
            Alien currentAlien = (Alien)element;
            
            gridLevel[row, col] = Instantiate(genericAlienPrefab, pos, Quaternion.identity, this.transform);
            SpriteRenderer currentSprite = gridLevel[row, col].GetComponent<SpriteRenderer>();

            gridLevel[row, col].GetComponent<CellView>().Initialize(new Vector2Int(row, col), id);

            // Get cosmetic
            foreach (CosmeticItemModel cosmetic in gameProgressionService.Cosmetics)
            {
                if (cosmetic.SelectedAliensIds.Contains(id))
                {
                    Addressables.LoadAssetAsync<Sprite>(gameConfigService.GetAlienInfo(id).Image + "_" + cosmetic.Name).Completed += handler =>
                    {
                        currentSprite.GetComponent<SpriteRenderer>().sprite = handler.Result;
                        Debug.Log(row + " " + col);
                    };
                    return;
                }
            }

            // Get default
            Addressables.LoadAssetAsync<Sprite>(gameConfigService.GetAlienInfo(id).Image).Completed += handler =>
            {
                currentSprite.GetComponent<SpriteRenderer>().sprite = handler.Result;
                Debug.Log(row + " " + col);
            };

            return;
        }
        else if(element is Booster)
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
