using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class controll the swap mechanic
 **/

public class SwapsController : MonoBehaviour
{
    public delegate void SwapDone(Element[,] grid);
    public delegate void CheckedMatch(Element element);
    public delegate void MoveDone();
    public static event SwapDone OnSwapDone = Grid => { };
    public static event CheckedMatch OnCheckedMatch;
    public static event MoveDone OnMoveDone;

    private Element elementSelected;
    private Element[,] gridLevel;
    private bool gridCreated = false, isPossibleSwap = true;

    private void Start()
    {
        LevelController.OnGridCreated += SetGrid;
        LevelController.OnGridChanged += SetGrid;

        InputController.OnElementSelected += SetElementSelected;
        InputController.OnElementMoved += CheckTryToMove;

        WinController.OnWinChecked += IsPossibleToSwap;
        LoseController.OnLoseChecked += IsPossibleToSwap;
    }

    private void OnDisable()
    {
        LevelController.OnGridCreated -= SetGrid;
        LevelController.OnGridChanged -= SetGrid;

        InputController.OnElementSelected -= SetElementSelected;
        InputController.OnElementMoved -= CheckTryToMove;

        WinController.OnWinChecked -= IsPossibleToSwap;
        LoseController.OnLoseChecked -= IsPossibleToSwap;
    }

    private void IsPossibleToSwap()
    {
        isPossibleSwap = false;
    }

    private void SetGrid(Element[,] grid)
    {
        gridLevel = grid;

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

        if(gridCreated == false)
        {
            gridCreated = true;
        }
    }

    private void SetElementSelected(GameObject element)
    {
        // Send positions
        if (gridLevel != null)
            elementSelected = gridLevel[(int)element.transform.position.x, (int)element.transform.position.y];
    }

    private void CheckTryToMove(Vector2 pos)
    {
        if (isPossibleSwap)
        {
            if (pos.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x <= -0.5) // Derecha
            {
                //Debug.Log("Derecha");
                if (IsOnLevel((int)pos.x + 1, (int)pos.y))
                {
                    StartCoroutine(SwapPositions((int)pos.x, (int)pos.y, (int)pos.x + 1, (int)pos.y));

                }
                elementSelected = null;

            }
            else if (pos.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= 0.5)  // Izquierda
            {
                //Debug.Log("Izquierda");
                if (IsOnLevel((int)pos.x - 1, (int)pos.y))
                {
                    StartCoroutine(SwapPositions((int)pos.x, (int)pos.y, (int)pos.x - 1, (int)pos.y));

                }
                elementSelected = null;

            }
            else if (pos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= -0.5) // Arriba
            {
                //Debug.Log("Arriba");
                if (IsOnLevel((int)pos.x, (int)pos.y + 1))
                {
                    StartCoroutine(SwapPositions((int)pos.x, (int)pos.y, (int)pos.x, (int)pos.y + 1));

                }
                elementSelected = null;

            }
            else if (pos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y >= 0.5)  // Abajo
            {
                //Debug.Log("Abajo");
                if (IsOnLevel((int)pos.x, (int)pos.y - 1))
                {
                    StartCoroutine(SwapPositions((int)pos.x, (int)pos.y, (int)pos.x, (int)pos.y - 1));

                }
                elementSelected = null;

            }
        }
    }

    // Swap the elements
    public IEnumerator SwapPositions(int row1, int col1, int row2, int col2)
    {
        if (IsAMatch(gridLevel[row1, col1], row2, col2) || IsAMatch(gridLevel[row2, col2], row1, col1))
        {
            Element aux = gridLevel[row1, col1];
            gridLevel[row1, col1] = gridLevel[row2, col2];
            gridLevel[row2, col2] = aux;

            gridLevel[row1, col1].SetPos(row1, col1);
            gridLevel[row2, col2].SetPos(row2, col2);

            yield return new WaitForSeconds(0);
            CheckMatch(gridLevel[row1, col1], row1, col1);
            yield return new WaitForSeconds(0);
            CheckMatch(gridLevel[row2, col2], row2, col2);
            if (gridCreated)
            {
                OnMoveDone();
            }
        }
    }

    // Check if there is a match
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

            res = true;
            OnSwapDone?.Invoke(gridLevel);
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

            res = true;
            OnSwapDone?.Invoke(gridLevel);
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

            res = true;
            OnSwapDone?.Invoke(gridLevel);
        }

        return res;
    }

    public bool IsOnLevel(int row, int col)
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
}
