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
    public delegate void CheckedMatch(Alien element);
    public static event SwapDone OnSwapDone = Grid => { };
    public event Action<Alien> OnCheckedMatch;
    public event Action OnMoveDone;

    private Element elementSelected;
    private bool gridCreated = false, isPossibleSwap = true;

    public static LevelController levelControllerInstance;

    public event Action<Element> OnCellCreated = delegate (Element element) { };
    public event Action<Element> OnCellDestroyed = delegate (Element element) { };
    public event Action<Element, Vector2Int> OnCellMoved = delegate (Element el, Vector2Int pos) { };
    public event Action<Element, Element> OnCellsSwapped = delegate (Element el1, Element el2) { };

    private WinController winController;
    private LoseController loseController;

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

        winController = new WinController(this);
        loseController = new LoseController(this);

        WinController.OnWinChecked += IsPossibleToSwap;
        LoseController.OnLoseChecked += IsPossibleToSwap;

    }

    public void CreateGrid()
    {
        GridChanged();
    }

    public void AddPoints(Element el)
    {
        OnCheckedMatch(el as Alien);
    }

    public void AddPoints(List<Element> els)
    {
        foreach (Element el in els)
            OnCheckedMatch(el as Alien);
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
                    gridModel.GridLevel[x, y] = new Alien(x, y, (AlienType)UnityEngine.Random.Range(0, colorTypes));
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
            if(gridModel.GridLevel[row, col] is Booster)
            {
                if (((Booster)gridModel.GridLevel[row, col]).GetElementType() == BoosterType.VerticalLineBooster)
                {
                    if (gridCreated)
                        OnCellDestroyed(gridModel.GridLevel[row, col]);
                    boosterController.FireVerticalLineBooster(new Vector2(row, col), moveBooster);
                    gridModel.GridLevel[row, col] = null;
                    return;
                }

                if (((Booster)gridModel.GridLevel[row, col]).GetElementType() == BoosterType.HorizontalLineBooster)
                {
                    if (gridCreated)
                        OnCellDestroyed(gridModel.GridLevel[row, col]);
                    boosterController.FireHorizontalLineBooster(new Vector2(row, col), moveBooster);
                    gridModel.GridLevel[row, col] = null;
                    return;
                }

                if (((Booster)gridModel.GridLevel[row, col]).GetElementType() == BoosterType.BombBooster)
                {
                    OnCellDestroyed(gridModel.GridLevel[row, col]);
                    boosterController.FireBombBooster(new Vector2(row, col), moveBooster);
                    gridModel.GridLevel[row, col] = null;
                    return;
                }

                if (((Booster)gridModel.GridLevel[row, col]).GetElementType() == BoosterType.ColorBombBooster)
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
            if(gridModel.GridLevel[row, col] is Booster)
            {
                if (((Booster)gridModel.GridLevel[row, col]).GetElementType() == BoosterType.VerticalLineBooster)
                {
                    if (gridCreated)
                        OnCellDestroyed(gridModel.GridLevel[row, col]);
                    boosterController.FireVerticalLineBooster(new Vector2(row, col), true);
                    gridModel.GridLevel[row, col] = null;
                    return;
                }

                if (((Booster)gridModel.GridLevel[row, col]).GetElementType() == BoosterType.HorizontalLineBooster)
                {
                    if (gridCreated)
                        OnCellDestroyed(gridModel.GridLevel[row, col]);
                    boosterController.FireHorizontalLineBooster(new Vector2(row, col), true);
                    gridModel.GridLevel[row, col] = null;
                    return;
                }

                if (((Booster)gridModel.GridLevel[row, col]).GetElementType() == BoosterType.BombBooster)
                {
                    OnCellDestroyed(gridModel.GridLevel[row, col]);
                    boosterController.FireBombBooster(new Vector2(row, col), true);
                    gridModel.GridLevel[row, col] = null;
                    return;
                }

                if (((Booster)gridModel.GridLevel[row, col]).GetElementType() == BoosterType.ColorBombBooster)
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
                while (gridModel.GridLevel[i, j] != null && gridModel.GridLevel[i, j] is Alien && IsAMatch(gridModel.GridLevel[i, j] as Alien, i, j))
                {
                    CheckMatch(gridModel.GridLevel[i, j] as Alien, i, j);
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
                if (gridModel.GridLevel[i, j]is Alien)
                    gridModel.GridLevel[i, j] = new Alien(i, j, (AlienType)UnityEngine.Random.Range(0, colorTypes));
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

        if (elementSelected != null && elementSelected is Booster)
        {
            if (((Booster)elementSelected).GetElementType() == BoosterType.VerticalLineBooster)
            {
                boosterController.FireVerticalLineBooster(element.transform.position, true);
                if (gridCreated)
                {
                    OnMoveDone();
                }
                return;
            }

            if (((Booster)elementSelected).GetElementType() == BoosterType.BombBooster)
            {
                boosterController.FireBombBooster(element.transform.position, true);
                if (gridCreated)
                {
                    OnMoveDone();
                }
                return;
            }

            if (((Booster)elementSelected).GetElementType() == BoosterType.HorizontalLineBooster)
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
                if (gridModel.GridLevel[i, j] is Booster)
                    return true;

                if (gridModel.IsOnGrid(i + 1, j))
                {
                    if (IsAMatch(gridModel.GridLevel[i, j] as Alien, i + 1, j))
                        return true;
                }

                if (gridModel.IsOnGrid(i - 1, j))
                {
                    if (IsAMatch(gridModel.GridLevel[i, j] as Alien, i - 1, j))
                        return true;
                }

                if (gridModel.IsOnGrid(i, j + 1))
                {
                    if (IsAMatch(gridModel.GridLevel[i, j] as Alien, i, j + 1))
                        return true;
                }

                if (gridModel.IsOnGrid(i, j - 1))
                {
                    if (IsAMatch(gridModel.GridLevel[i, j] as Alien, i, j - 1))
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
        if(gridModel.GridLevel[row1, col1] is Booster)
        {
            if (((Booster)gridModel.GridLevel[row1, col1]).GetElementType() == BoosterType.ColorBombBooster)
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

            if (((Booster)gridModel.GridLevel[row2, col2]).GetElementType() == BoosterType.ColorBombBooster)
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
        }else if (IsAMatch(gridModel.GridLevel[row1, col1] as Alien, row2, col2) || IsAMatch(gridModel.GridLevel[row2, col2] as Alien, row1, col1))
        {
            OnCellsSwapped(gridModel.GridLevel[row1, col1], gridModel.GridLevel[row2, col2]);

            Element aux = gridModel.GridLevel[row1, col1];
            gridModel.GridLevel[row1, col1] = gridModel.GridLevel[row2, col2];
            gridModel.GridLevel[row2, col2] = aux;

            gridModel.GridLevel[row1, col1].SetPos(row1, col1);
            gridModel.GridLevel[row2, col2].SetPos(row2, col2);

            CheckMatch(gridModel.GridLevel[row1, col1] as Alien, row1, col1);
            CheckMatch(gridModel.GridLevel[row2, col2] as Alien, row2, col2);
            MoveDownPieces();
            if (gridCreated)
            {
                OnMoveDone();
            }
        }

    }

    // Check if there is a match with the element in the position row col
    public bool IsAMatch(Alien element, int row, int col)
    {
        if (element == null || element is not Alien)
            return false;

        bool res = false;

        bool sameColor = true;
        int pos = 1, nSameColor = 1;
        AlienType elementColor = element.GetElementType();

        // Check horizontal
        while (sameColor)
        {
            if (gridModel.IsOnGrid(row - pos, col) && row - pos != element.GetPosX() && gridModel.GridLevel[row - pos, col] is Alien)
            {
                if (elementColor == ((Alien)gridModel.GridLevel[row - pos, col]).GetElementType())
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
            if (gridModel.IsOnGrid(row + pos, col) && row + pos != element.GetPosX() && gridModel.GridLevel[row + pos, col] is Alien)
            {
                if (elementColor == ((Alien)gridModel.GridLevel[row + pos, col]).GetElementType())
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
            if (gridModel.IsOnGrid(row, col - pos) && col - pos != element.GetPosY() && gridModel.GridLevel[row, col - pos] is Alien)
            {
                if (elementColor == ((Alien)gridModel.GridLevel[row, col - pos]).GetElementType())
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
            if (gridModel.IsOnGrid(row, col + pos) && col + pos != element.GetPosY() && gridModel.GridLevel[row, col + pos] is Alien)
            {
                if (elementColor == ((Alien)gridModel.GridLevel[row, col + pos]).GetElementType())
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
    private bool CheckMatch(Alien element, int row, int col)
    {
        if (element == null || element is not Alien)
            return false;

        bool res = false;

        List<Element> sameColorVertical = new List<Element>();
        List<Element> sameColorHorizontal = new List<Element>();

        sameColorVertical.Add(element);
        sameColorHorizontal.Add(element);

        bool sameColor = true;
        int pos = 1;
        AlienType elementColor = element.GetElementType();

        // Check horizontal
        while (sameColor)
        {
            if (gridModel.IsOnGrid(row - pos, col) && gridModel.GridLevel[row - pos, col] is Alien)
            {
                if (elementColor == ((Alien)gridModel.GridLevel[row - pos, col]).GetElementType())
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
            if (gridModel.IsOnGrid(row + pos, col) && gridModel.GridLevel[row + pos, col] is Alien)
            {
                if (elementColor == ((Alien)gridModel.GridLevel[row + pos, col]).GetElementType())
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
            if (gridModel.IsOnGrid(row, col - pos) && gridModel.GridLevel[row, col - pos] is Alien)
            {
                if (elementColor == ((Alien)gridModel.GridLevel[row, col - pos]).GetElementType())
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
            if (gridModel.IsOnGrid(row, col + pos) && gridModel.GridLevel[row, col + pos] is Alien)
            {
                if (elementColor == ((Alien)gridModel.GridLevel[row, col + pos]).GetElementType())
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
                    OnCheckedMatch(sameColorHorizontal[i] as Alien);
                }
                DestroyCell((int)sameColorHorizontal[i].GetPosX(), (int)sameColorHorizontal[i].GetPosY());
            }

            for (int i = sameColorVertical.Count - 1; i > 0; i--)
            {
                if (gridCreated)
                {
                    OnCheckedMatch(sameColorVertical[i] as Alien);
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
                    OnCheckedMatch(sameColorHorizontal[i] as Alien);
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
                    OnCheckedMatch(sameColorVertical[i] as Alien);
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
