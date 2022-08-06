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
    private static Element[,] gridLevel;

    public delegate void SwapDone(Element[,] grid);
    public delegate void CheckedMatch(Element element);
    public delegate void MoveDone();
    public static event SwapDone OnSwapDone = Grid => { };
    public static event CheckedMatch OnCheckedMatch;
    public static event MoveDone OnMoveDone;

    private Element elementSelected;
    private bool gridCreated = false, isPossibleSwap = true;
    private IBooster actualBooster;

    public LevelController(int width = 9, int height = 9, int colorTypes = 6)
    {
        gridModel = new Grid(width, height, colorTypes);
        gridLevel = gridModel.GetGridLevel();
        

        WinController.OnWinChecked += IsPossibleToSwap;
        LoseController.OnLoseChecked += IsPossibleToSwap;

        ActiveBoosterController.OnBoosterActived += FireBooster;
    }

    public void CreateGrid()
    {
        OnGridCreated(gridLevel);
        GridChanged();
    }

    public void FireBooster(IBooster booster, Vector2 pos)
    {
        booster.Execute(pos, ref gridLevel);
        MoveDownPieces();
    }


    // Move all the pieces down if it is possible
    private void MoveDownPieces()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gridLevel[x, y] == null)
                {
                    for (int yAux = y + 1; yAux < height; yAux++)
                    {
                        if (gridLevel[x, yAux] != null)
                        {
                            gridLevel[x, y] = gridLevel[x, yAux];
                            gridLevel[x, yAux] = null;
                            gridLevel[x, y].SetPos(x,y);
                            break;
                        }
                    }
                }
            }
        }

        gridModel.SetGridLevel(gridLevel);
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
                if (gridLevel[x, y] == null)
                {
                    gridLevel[x, y] = new Element(x, y, UnityEngine.Random.Range(0, colorTypes));
                }
            }
        }

        gridModel.SetGridLevel(gridLevel);
        OnGridChanged(gridLevel);
    }

    private void CreateLineBooster(int row, int col)
    {
        gridLevel[row, col] = new Element(row, col, 6);
    }

    private void CreateBombBooster(int row, int col)
    {
        gridLevel[row, col] = new Element(row, col, 7);
    }

    private void CreateColorBombBooster(int row, int col)
    {
        gridLevel[row, col] = new Element(row, col, 8);
    }

    private void FireLineBooster(Vector2 pos)
    {
        actualBooster = new CrossLineBooster();
        actualBooster.Execute(pos, ref gridLevel);
        actualBooster = null;
        MoveDownPieces();
    }

    private void FireBombBooster(Vector2 pos)
    {
        actualBooster = new BombBooster();
        actualBooster.Execute(pos, ref gridLevel);
        actualBooster = null;
        MoveDownPieces();
    }

    private void FireColorBombBooster(Vector2 pos)
    {
        actualBooster = new ColorBombBooster();
        actualBooster.Execute(pos, ref gridLevel);
        actualBooster = null;
        MoveDownPieces();
    }

    /************************************************************************************************************/
    // Check all the grid to check all the matches existing
    private void GridChanged()
    {

        for (int i = 0; i < gridLevel.GetLength(0); i++)
        {
            for (int j = 0; j < gridLevel.GetLength(1); j++)
            {
                while (IsAMatch(gridLevel[i, j], i, j))
                {
                    CheckMatch(gridLevel[i, j], i, j);
                }
            }
        }

        if (!CheckPossibleMatch())
        {
            Debug.Log("No hay match posible");
        }

        if (gridCreated == false)
        {
            gridCreated = true;
        }

        
    }


    public void SetElementSelected(GameObject element)
    {
        // Send positions
        if (gridLevel != null)
            elementSelected = gridLevel[(int)element.transform.position.x, (int)element.transform.position.y];
        else
            return;

        if(elementSelected.GetColorType() == 6)
        {
            FireLineBooster(element.transform.position);
            return;
        }

        if (elementSelected.GetColorType() == 7)
        {
            FireBombBooster(element.transform.position);
            return;
        }
    }

    // Return true if it is possible for the player make a match
    private bool CheckPossibleMatch()
    {
        for (int i = 0; i < gridLevel.GetLength(0); i++)
        {
            for (int j = 0; j < gridLevel.GetLength(1); j++)
            {
                if (IsOnLevel(i + 1, j))
                {
                    if (IsAMatch(gridLevel[i, j], i + 1, j))
                        return true;
                }

                if (IsOnLevel(i - 1, j))
                {
                    if (IsAMatch(gridLevel[i, j], i - 1, j))
                        return true;
                }

                if (IsOnLevel(i, j + 1))
                {
                    if (IsAMatch(gridLevel[i, j], i, j + 1))
                        return true;
                }

                if (IsOnLevel(i, j - 1))
                {
                    if (IsAMatch(gridLevel[i, j], i, j - 1))
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
                if (IsOnLevel((int)pos.x + 1, (int)pos.y))
                {
                    SwapPositions((int)pos.x, (int)pos.y, (int)pos.x + 1, (int)pos.y);

                }
                elementSelected = null;

            }
            else if (pos.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= 0.5)  // Izquierda
            {
                //Debug.Log("Izquierda");
                if (IsOnLevel((int)pos.x - 1, (int)pos.y))
                {
                    SwapPositions((int)pos.x, (int)pos.y, (int)pos.x - 1, (int)pos.y);

                }
                elementSelected = null;

            }
            else if (pos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= -0.5) // Arriba
            {
                //Debug.Log("Arriba");
                if (IsOnLevel((int)pos.x, (int)pos.y + 1))
                {
                    SwapPositions((int)pos.x, (int)pos.y, (int)pos.x, (int)pos.y + 1);

                }
                elementSelected = null;

            }
            else if (pos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y >= 0.5)  // Abajo
            {
                //Debug.Log("Abajo");
                if (IsOnLevel((int)pos.x, (int)pos.y - 1))
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
        if (gridLevel[row1,col1].GetColorType() == 8)
        {
            FireColorBombBooster(new Vector2(row2, col2));
            gridLevel[row1, col1] = null;
            MoveDownPieces();
            return;
        }

        if (gridLevel[row2, col2].GetColorType() == 8)
        {
            FireColorBombBooster(new Vector2(row1, col1));
            gridLevel[row2, col2] = null;
            MoveDownPieces();
            return;
        }

        if (IsAMatch(gridLevel[row1, col1], row2, col2) || IsAMatch(gridLevel[row2, col2], row1, col1))
        {
            Element aux = gridLevel[row1, col1];
            gridLevel[row1, col1] = gridLevel[row2, col2];
            gridLevel[row2, col2] = aux;

            gridLevel[row1, col1].SetPos(row1, col1);
            gridLevel[row2, col2].SetPos(row2, col2);

            CheckMatch(gridLevel[row1, col1], row1, col1);
            CheckMatch(gridLevel[row2, col2], row2, col2);
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
            if (IsOnLevel(row - pos, col))
            {
                if (elementColor == gridLevel[row - pos, col].GetColorType())
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
            if (IsOnLevel(row + pos, col))
            {
                if (elementColor == gridLevel[row + pos, col].GetColorType())
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

            if (pos >= 3)
                return true;
        }

        sameColor = true;
        pos = 1;
        nSameColor = 1;

        // Check vertical
        while (sameColor)
        {
            if (IsOnLevel(row, col - pos))
            {
                if (elementColor == gridLevel[row, col - pos].GetColorType())
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
            if (IsOnLevel(row, col + pos))
            {
                if (elementColor == gridLevel[row, col + pos].GetColorType())
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
            if (IsOnLevel(row - pos, col))
            {
                if (elementColor == gridLevel[row - pos, col].GetColorType())
                {
                    sameColorHorizontal.Add(gridLevel[row - pos, col]);
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
            if (IsOnLevel(row + pos, col))
            {
                if (elementColor == gridLevel[row + pos, col].GetColorType())
                {
                    sameColorHorizontal.Add(gridLevel[row + pos, col]);
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
            if (IsOnLevel(row, col - pos))
            {
                if (elementColor == gridLevel[row, col - pos].GetColorType())
                {
                    sameColorVertical.Add(gridLevel[row, col - pos]);
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
            if (IsOnLevel(row, col + pos))
            {
                if (elementColor == gridLevel[row, col + pos].GetColorType())
                {
                    sameColorVertical.Add(gridLevel[row, col + pos]);
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
                gridLevel[(int)sameColorHorizontal[i].GetPosX(), (int)sameColorHorizontal[i].GetPosY()] = null;
            }

            for (int i = sameColorVertical.Count - 1; i > 0; i--)
            {
                if (gridCreated)
                {
                    OnCheckedMatch(sameColorVertical[i]);
                }
                gridLevel[(int)sameColorVertical[i].GetPosX(), (int)sameColorVertical[i].GetPosY()] = null;
            }

            if(gridCreated)
                CreateBombBooster(row, col);

            res = true;
            //OnSwapDone?.Invoke(gridLevel);
            MoveDownPieces();
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
                gridLevel[(int)sameColorHorizontal[i].GetPosX(), (int)sameColorHorizontal[i].GetPosY()] = null;
            }

            if (gridCreated)
            {
                if (sameColorHorizontal.Count == 4)
                    CreateLineBooster(row, col);

                if (sameColorHorizontal.Count >= 5)
                    CreateColorBombBooster(row, col);
            }
            

            res = true;
            //OnSwapDone?.Invoke(gridLevel);
            MoveDownPieces();
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
                gridLevel[(int)sameColorVertical[i].GetPosX(), (int)sameColorVertical[i].GetPosY()] = null;
            }

            if (gridCreated)
            {
                if (sameColorVertical.Count == 4)
                    CreateLineBooster(row, col);

                if (sameColorVertical.Count >= 5)
                    CreateColorBombBooster(row, col);
            }
            

            res = true;
            //OnSwapDone?.Invoke(gridLevel);
            MoveDownPieces();
        }

        return res;
    }

    public static bool IsOnLevel(int row, int col)
    {
        if (row < gridLevel.GetLength(0) && col < gridLevel.GetLength(1) && row >= 0 && col >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void IsPossibleToSwap()
    {
        isPossibleSwap = false;
    }
}
