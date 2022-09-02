using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manage the movement of the elements in the level
 * */

public class LevelController
{
    public delegate void GridCreated(Element[,] grid);
    public static event GridCreated OnGridCreated;
    public static event GridCreated OnGridChanged;

    public int width = 9, height = 9, colorTypes = 6;
    private Grid gridModel;
    private BoosterController boosterController;

    public delegate void SwapDone(Element[,] grid);
    public delegate void CheckedMatch(Element element);
    public delegate void MoveDone();
    public static event SwapDone OnSwapDone = Grid => { };
    public static event CheckedMatch OnCheckedMatch;
    public static event MoveDone OnMoveDone;

    private Element elementSelected;
    private bool gridCreated = false, isPossibleSwap = true;

    public static LevelController levelControllerInstance;

    public event Action<Element> OnCellCreated = delegate (Element element) { };
    public event Action<Element> OnCellDestroyed = delegate (Element element) { };
    public event Action<Element, Vector2Int> OnCellMoved = delegate (Element el, Vector2Int pos) { };
    public event Action<Element, Element> OnCellsSwapped = delegate (Element el1, Element el2) { };

    public void DestroyCell(Element el) => OnCellDestroyed(el);
    public void CreateCell(Element el) => OnCellCreated(el);

    public LevelController(int width = 9, int height = 9, int colorTypes = 6)
    {
        this.width = width;
        this.height = height;
        this.colorTypes = colorTypes;

        gridModel = new Grid(width, height, colorTypes);
        boosterController = new BoosterController(gridModel, this);

        levelControllerInstance = this;

        WinController.OnWinChecked += IsPossibleToSwap;
        LoseController.OnLoseChecked += IsPossibleToSwap;

    }

    public void CreateGrid()
    {
        GridChanged();
    }

    public void AddPoints(Element el)
    {
        OnCheckedMatch(el);
    }

    public void AddPoints(List<Element> els)
    {
        foreach (Element el in els)
            OnCheckedMatch(el);
    }


    // Move all the pieces down if it is possible
    public void MoveDownPieces()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gridModel.GridLevel[x, y] == null)
                {
                    for (int yAux = y + 1; yAux < height; yAux++)
                    {
                        if (gridModel.GridLevel[x, yAux] != null)
                        {
                            gridModel.GridLevel[x, y] = gridModel.GridLevel[x, yAux];
                            gridModel.GridLevel[x, yAux] = null;

                            if (gridCreated)
                            {
                                OnCellMoved(gridModel.GridLevel[x, y], new Vector2Int(x, y));
                            }

                            gridModel.GridLevel[x, y].SetPos(x, y);
                            break;
                        }
                    }
                }
            }
        }

        gridModel.SetGridLevel(gridModel.GridLevel);
        FillBlanks();
        GridChanged();
    }

    // Fill all the places where the cell is null
    private void FillBlanks()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gridModel.GridLevel[x, y] == null)
                {
                    gridModel.GridLevel[x, y] = new Element(x, y, UnityEngine.Random.Range(0, colorTypes));
                    if (gridCreated)
                    {
                        OnCellCreated(gridModel.GridLevel[x, y]);
                    }
                }
            }
        }

        gridModel.SetGridLevel(gridModel.GridLevel);

        if (gridCreated)
        {
            //OnGridChanged(gridModel.GridLevel);
        }
    }

    public void DestroyCell(int row, int col, bool moveBooster)
    {
        if (gridModel.GridLevel[row, col] != null)
        {
            if (gridModel.GridLevel[row, col].GetColorType() == 6)
            {
                if (gridCreated)
                    OnCellDestroyed(gridModel.GridLevel[row, col]);
                boosterController.FireVerticalLineBooster(new Vector2(row, col), moveBooster);
                gridModel.GridLevel[row, col] = null;
                return;
            }

            if (gridModel.GridLevel[row, col].GetColorType() == 7)
            {
                OnCellDestroyed(gridModel.GridLevel[row, col]);
                boosterController.FireBombBooster(new Vector2(row, col), moveBooster);
                gridModel.GridLevel[row, col] = null;
                return;
            }

            if (gridModel.GridLevel[row, col].GetColorType() == 8)
            {
                if (gridCreated)
                    OnCellDestroyed(gridModel.GridLevel[row, col]);
                if (gridModel.IsOnGrid(row + 1, col))
                    boosterController.FireColorBombBooster(gridModel.GridLevel[row, col], new Vector2(row + 1, col), moveBooster);
                else if (gridModel.IsOnGrid(row, col + 1))
                    boosterController.FireColorBombBooster(gridModel.GridLevel[row, col], new Vector2(row, col + 1), moveBooster);
                else if (gridModel.IsOnGrid(row, col - 1))
                    boosterController.FireColorBombBooster(gridModel.GridLevel[row, col], new Vector2(row, col - 1), moveBooster);
                else if (gridModel.IsOnGrid(row - 1, col))
                    boosterController.FireColorBombBooster(gridModel.GridLevel[row, col], new Vector2(row - 1, col), moveBooster);

                gridModel.GridLevel[row, col] = null;
                return;
            }

            if (gridModel.GridLevel[row, col].GetColorType() == 9)
            {
                if (gridCreated)
                    OnCellDestroyed(gridModel.GridLevel[row, col]);
                boosterController.FireHorizontalLineBooster(new Vector2(row, col), moveBooster);
                gridModel.GridLevel[row, col] = null;
                return;
            }
            if (gridCreated)
                OnCellDestroyed(gridModel.GridLevel[row, col]);
            gridModel.GridLevel[row, col] = null;
        }
    }

    public void DestroyCell(int row, int col)
    {
        if (gridModel.GridLevel[row, col] != null)
        {
            if (gridModel.GridLevel[row, col].GetColorType() == 6)
            {
                if (gridCreated)
                    OnCellDestroyed(gridModel.GridLevel[row, col]);
                boosterController.FireVerticalLineBooster(new Vector2(row, col), true);
                gridModel.GridLevel[row, col] = null;
                return;
            }

            if (gridModel.GridLevel[row, col].GetColorType() == 7)
            {
                OnCellDestroyed(gridModel.GridLevel[row, col]);
                boosterController.FireBombBooster(new Vector2(row, col), true);
                gridModel.GridLevel[row, col] = null;
                return;
            }

            if (gridModel.GridLevel[row, col].GetColorType() == 8)
            {
                if (gridCreated)
                    OnCellDestroyed(gridModel.GridLevel[row, col]);
                if (gridModel.IsOnGrid(row + 1, col))
                    boosterController.FireColorBombBooster(gridModel.GridLevel[row, col], new Vector2(row + 1, col), true);
                else if (gridModel.IsOnGrid(row, col + 1))
                    boosterController.FireColorBombBooster(gridModel.GridLevel[row, col], new Vector2(row, col + 1), true);
                else if (gridModel.IsOnGrid(row, col - 1))
                    boosterController.FireColorBombBooster(gridModel.GridLevel[row, col], new Vector2(row, col - 1), true);
                else if (gridModel.IsOnGrid(row - 1, col))
                    boosterController.FireColorBombBooster(gridModel.GridLevel[row, col], new Vector2(row - 1, col), true);

                gridModel.GridLevel[row, col] = null;
                return;
            }

            if (gridModel.GridLevel[row, col].GetColorType() == 9)
            {
                if (gridCreated)
                    OnCellDestroyed(gridModel.GridLevel[row, col]);
                boosterController.FireHorizontalLineBooster(new Vector2(row, col), true);
                gridModel.GridLevel[row, col] = null;
                return;
            }
            if (gridCreated)
                OnCellDestroyed(gridModel.GridLevel[row, col]);
            gridModel.GridLevel[row, col] = null;
        }
    }

    /************************************************************************************************************/
    // Check all the grid to check all the matches existing
    private void GridChanged()
    {
        bool existMatch = false;

        for (int i = 0; i < gridModel.GridLevel.GetLength(0); i++)
        {
            for (int j = 0; j < gridModel.GridLevel.GetLength(1); j++)
            {
                while (gridModel.GridLevel[i, j] != null && IsAMatch(gridModel.GridLevel[i, j], i, j))
                {
                    CheckMatch(gridModel.GridLevel[i, j], i, j);
                    existMatch = true;
                }
            }
        }

        if (existMatch)
            MoveDownPieces();

        if (!CheckPossibleMatch())
        {
            Debug.Log("No hay match posible");
            ReGenerateGrid();
        }

        if (gridCreated == false)
        {
            OnGridCreated(gridModel.GridLevel);
            gridCreated = true;
        }


    }

    private void ReGenerateGrid()
    {
        for (int i = 0; i < gridModel.GridLevel.GetLength(0); i++)
        {
            for (int j = 0; j < gridModel.GridLevel.GetLength(1); j++)
            {
                if (gridModel.GridLevel[i, j].GetColorType() < colorTypes - 1)
                    gridModel.GridLevel[i, j] = new Element(i, j, UnityEngine.Random.Range(0, colorTypes));
            }
        }

        OnGridChanged(gridModel.GridLevel);
    }

    public void SetElementSelected(GameObject element)
    {
        // Send positions
        if (gridModel.GridLevel != null)
            elementSelected = gridModel.GridLevel[(int)element.transform.position.x, (int)element.transform.position.y];
        else
            return;

        if (elementSelected != null)
        {
            if (elementSelected.GetColorType() == 6)
            {
                boosterController.FireVerticalLineBooster(element.transform.position, true);
                if (gridCreated)
                {
                    OnMoveDone();
                }
                return;
            }

            if (elementSelected.GetColorType() == 7)
            {
                boosterController.FireBombBooster(element.transform.position, true);
                if (gridCreated)
                {
                    OnMoveDone();
                }
                return;
            }

            if (elementSelected.GetColorType() == 9)
            {
                boosterController.FireHorizontalLineBooster(element.transform.position, true);
                if (gridCreated)
                {
                    OnMoveDone();
                }
                return;
            }
        }
    }

    // Return true if it is possible for the player make a match
    private bool CheckPossibleMatch()
    {
        for (int i = 0; i < gridModel.GridLevel.GetLength(0); i++)
        {
            for (int j = 0; j < gridModel.GridLevel.GetLength(1); j++)
            {
                // Check if it is a booster
                if (gridModel.GridLevel[i, j].GetColorType() > 5)
                    return true;

                if (gridModel.IsOnGrid(i + 1, j))
                {
                    if (IsAMatch(gridModel.GridLevel[i, j], i + 1, j))
                        return true;
                }

                if (gridModel.IsOnGrid(i - 1, j))
                {
                    if (IsAMatch(gridModel.GridLevel[i, j], i - 1, j))
                        return true;
                }

                if (gridModel.IsOnGrid(i, j + 1))
                {
                    if (IsAMatch(gridModel.GridLevel[i, j], i, j + 1))
                        return true;
                }

                if (gridModel.IsOnGrid(i, j - 1))
                {
                    if (IsAMatch(gridModel.GridLevel[i, j], i, j - 1))
                        return true;
                }
            }
        }

        return false;
    }

    // Check if the movement done by the player is resolving a match
    public void CheckTryToMove(Vector2 pos)
    {
        if (isPossibleSwap)
        {
            if (pos.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x <= -0.5) // Derecha
            {
                //Debug.Log("Derecha");
                if (gridModel.IsOnGrid((int)pos.x + 1, (int)pos.y))
                {
                    SwapPositions((int)pos.x, (int)pos.y, (int)pos.x + 1, (int)pos.y);

                }
                elementSelected = null;

            }
            else if (pos.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= 0.5)  // Izquierda
            {
                //Debug.Log("Izquierda");
                if (gridModel.IsOnGrid((int)pos.x - 1, (int)pos.y))
                {
                    SwapPositions((int)pos.x, (int)pos.y, (int)pos.x - 1, (int)pos.y);

                }
                elementSelected = null;

            }
            else if (pos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= -0.5) // Arriba
            {
                //Debug.Log("Arriba");
                if (gridModel.IsOnGrid((int)pos.x, (int)pos.y + 1))
                {
                    SwapPositions((int)pos.x, (int)pos.y, (int)pos.x, (int)pos.y + 1);

                }
                elementSelected = null;

            }
            else if (pos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y >= 0.5)  // Abajo
            {
                //Debug.Log("Abajo");
                if (gridModel.IsOnGrid((int)pos.x, (int)pos.y - 1))
                {
                    SwapPositions((int)pos.x, (int)pos.y, (int)pos.x, (int)pos.y - 1);

                }
                elementSelected = null;

            }
        }
    }

    // Swap the elements
    public void SwapPositions(int row1, int col1, int row2, int col2)
    {
        if (gridModel.GridLevel[row1, col1].GetColorType() == 8)
        {
            boosterController.FireColorBombBooster(gridModel.GridLevel[row1, col1], new Vector2(row2, col2), true);
            if (gridCreated)
            {
                OnMoveDone();
            }
            gridModel.GridLevel[row1, col1] = null;
            MoveDownPieces();
            return;
        }

        if (gridModel.GridLevel[row2, col2].GetColorType() == 8)
        {
            boosterController.FireColorBombBooster(gridModel.GridLevel[row1, col1], new Vector2(row1, col1), true);
            if (gridCreated)
            {
                OnMoveDone();
            }
            gridModel.GridLevel[row2, col2] = null;
            MoveDownPieces();
            return;
        }


        if (IsAMatch(gridModel.GridLevel[row1, col1], row2, col2) || IsAMatch(gridModel.GridLevel[row2, col2], row1, col1))
        {
            OnCellsSwapped(gridModel.GridLevel[row1, col1], gridModel.GridLevel[row2, col2]);

            Element aux = gridModel.GridLevel[row1, col1];
            gridModel.GridLevel[row1, col1] = gridModel.GridLevel[row2, col2];
            gridModel.GridLevel[row2, col2] = aux;

            gridModel.GridLevel[row1, col1].SetPos(row1, col1);
            gridModel.GridLevel[row2, col2].SetPos(row2, col2);

            CheckMatch(gridModel.GridLevel[row1, col1], row1, col1);
            CheckMatch(gridModel.GridLevel[row2, col2], row2, col2);
            MoveDownPieces();
            if (gridCreated)
            {
                OnMoveDone();
            }
        }

    }

    // Check if there is a match with the element in the position row col
    public bool IsAMatch(Element element, int row, int col)
    {
        bool res = false;

        bool sameColor = true;
        int pos = 1, nSameColor = 1;
        int elementColor = element.GetColorType();

        // Check horizontal
        while (sameColor)
        {
            if (gridModel.IsOnGrid(row - pos, col) && row - pos != element.GetPosX())
            {
                if (elementColor == gridModel.GridLevel[row - pos, col].GetColorType())
                {
                    pos++;
                    nSameColor++;
                }
                else
                {
                    sameColor = false;
                }
            }
            else
            {
                sameColor = false;
            }

            if (nSameColor >= 3)
                return true;

        }

        sameColor = true;
        pos = 1;

        while (sameColor)
        {
            if (gridModel.IsOnGrid(row + pos, col) && row + pos != element.GetPosX())
            {
                if (elementColor == gridModel.GridLevel[row + pos, col].GetColorType())
                {
                    pos++;
                    nSameColor++;
                }
                else
                {
                    sameColor = false;
                }
            }
            else
            {
                sameColor = false;
            }

            if (nSameColor >= 3)
                return true;
        }

        sameColor = true;
        pos = 1;
        nSameColor = 1;

        // Check vertical
        while (sameColor)
        {
            if (gridModel.IsOnGrid(row, col - pos) && col - pos != element.GetPosY())
            {
                if (elementColor == gridModel.GridLevel[row, col - pos].GetColorType())
                {
                    pos++;
                    nSameColor++;
                }
                else
                {
                    sameColor = false;
                }
            }
            else
            {
                sameColor = false;
            }

            if (nSameColor >= 3)
                return true;
        }

        sameColor = true;
        pos = 1;

        while (sameColor)
        {
            if (gridModel.IsOnGrid(row, col + pos) && col + pos != element.GetPosY())
            {
                if (elementColor == gridModel.GridLevel[row, col + pos].GetColorType())
                {
                    pos++;
                    nSameColor++;
                }
                else
                {
                    sameColor = false;
                }
            }
            else
            {
                sameColor = false;
            }

            if (nSameColor >= 3)
                return true;
        }

        return res;
    }


    //enum CheckDirection
    //{
    //    ToLeft,
    //    ToRight,
    //    ToUp,
    //    ToDown,
    //}

    //Vector2Int[] OffsetByDirection = 
    //{
    //    new Vector2Int {-1, 0},
    //    new Vector2Int { 1, 0 },
    //    new Vector2Int { 0, -1 },
    //    new Vector2Int { 0, 1 },
    //};

    // Check the match of the element given and make it
    private bool CheckMatch(Element element, int row, int col)
    {
        bool res = false;

        List<Element> sameColorVertical = new List<Element>();
        List<Element> sameColorHorizontal = new List<Element>();

        sameColorVertical.Add(element);
        sameColorHorizontal.Add(element);

        bool sameColor = true;
        int pos = 1;
        int elementColor = element.GetColorType();

        // Check horizontal
        while (sameColor)
        {
            if (gridModel.IsOnGrid(row - pos, col))
            {
                if (elementColor == gridModel.GridLevel[row - pos, col].GetColorType())
                {
                    sameColorHorizontal.Add(gridModel.GridLevel[row - pos, col]);
                    pos++;
                }
                else
                {
                    sameColor = false;
                }
            }
            else
            {
                sameColor = false;
            }

        }

        sameColor = true;
        pos = 1;

        while (sameColor)
        {
            if (gridModel.IsOnGrid(row + pos, col))
            {
                if (elementColor == gridModel.GridLevel[row + pos, col].GetColorType())
                {
                    sameColorHorizontal.Add(gridModel.GridLevel[row + pos, col]);
                    pos++;
                }
                else
                {
                    sameColor = false;
                }
            }
            else
            {
                sameColor = false;
            }
        }

        sameColor = true;
        pos = 1;

        // Check vertical
        while (sameColor)
        {
            if (gridModel.IsOnGrid(row, col - pos))
            {
                if (elementColor == gridModel.GridLevel[row, col - pos].GetColorType())
                {
                    sameColorVertical.Add(gridModel.GridLevel[row, col - pos]);
                    pos++;
                }
                else
                {
                    sameColor = false;
                }
            }
            else
            {
                sameColor = false;
            }
        }

        sameColor = true;
        pos = 1;

        while (sameColor)
        {
            if (gridModel.IsOnGrid(row, col + pos))
            {
                if (elementColor == gridModel.GridLevel[row, col + pos].GetColorType())
                {
                    sameColorVertical.Add(gridModel.GridLevel[row, col + pos]);
                    pos++;
                }
                else
                {
                    sameColor = false;
                }
            }
            else
            {
                sameColor = false;
            }
        }

        if (sameColorHorizontal.Count >= 3 && sameColorVertical.Count >= 3)
        {
            for (int i = sameColorHorizontal.Count - 1; i >= 0; i--)
            {
                if (gridCreated)
                {
                    OnCheckedMatch(sameColorHorizontal[i]);
                }
                DestroyCell((int)sameColorHorizontal[i].GetPosX(), (int)sameColorHorizontal[i].GetPosY());
            }

            for (int i = sameColorVertical.Count - 1; i > 0; i--)
            {
                if (gridCreated)
                {
                    OnCheckedMatch(sameColorVertical[i]);
                }
                DestroyCell((int)sameColorVertical[i].GetPosX(), (int)sameColorVertical[i].GetPosY());
            }

            res = true;
        }
        else if (sameColorHorizontal.Count >= 3)
        {
            Debug.Log("Horizontal de " + sameColorHorizontal.Count);
            for (int i = sameColorHorizontal.Count - 1; i >= 0; i--)
            {
                if (gridCreated)
                {
                    OnCheckedMatch(sameColorHorizontal[i]);
                }
                DestroyCell((int)sameColorHorizontal[i].GetPosX(), (int)sameColorHorizontal[i].GetPosY());
            }

            res = true;
        }
        else if (sameColorVertical.Count >= 3)
        {
            Debug.Log("Vertical de " + sameColorVertical.Count);
            for (int i = sameColorVertical.Count - 1; i >= 0; i--)
            {
                if (gridCreated)
                {
                    OnCheckedMatch(sameColorVertical[i]);
                }
                DestroyCell((int)sameColorVertical[i].GetPosX(), (int)sameColorVertical[i].GetPosY());
            }

            res = true;
        }

        if(gridCreated)
            boosterController.TryCreateBooster(sameColorHorizontal.Count, sameColorVertical.Count, row, col);
        
        return res;
    }

    private void IsPossibleToSwap()
    {
        isPossibleSwap = false;
    }
}
