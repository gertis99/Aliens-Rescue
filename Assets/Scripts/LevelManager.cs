using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns the elements in the grid

public class LevelManager : MonoBehaviour
{

    public GameObject gridGO;
    public GameObject[] levelElemnts;
    private GameObject[,] gridLevel;
    public int width, height;

    // Start is called before the first frame update
    void Start()
    {
        gridLevel = new GameObject[width, height];
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateLevel()
    {
        for (int i=0; i< gridLevel.GetLength(0); i++)
        {
            for(int j=0; j< gridLevel.GetLength(1); j++)
            {
                gridLevel[i, j] = Instantiate(levelElemnts[UnityEngine.Random.Range(0, levelElemnts.Length - 1)], new Vector2(i, j), Quaternion.identity);
                gridLevel[i, j].transform.SetParent(this.transform, true);
                gridLevel[i, j].GetComponent<ElementManager>().SetPos(i, j);
            }
        }

        for (int i = 0; i < gridLevel.GetLength(0); i++)
        {
            for (int j = 0; j < gridLevel.GetLength(1); j++)
            {
                while(CheckMatch(gridLevel[i, j], i, j))
                {

                }
            }
        }
    }

    public GameObject[,] GetGridLevel()
    {
        return gridLevel;
    }

    // Swap the elements
    public IEnumerator SwapPositions(int row1, int col1, int row2, int col2)
    {
        if(IsAMatch(gridLevel[row1, col1], row2, col2) || IsAMatch(gridLevel[row2, col2], row1, col1))
        {
            GameObject aux = gridLevel[row1, col1];
            gridLevel[row1, col1] = gridLevel[row2, col2];
            gridLevel[row2, col2] = aux;

            gridLevel[row1, col1].transform.position = new Vector2(row1, col1);
            gridLevel[row2, col2].transform.position = new Vector2(row2, col2);

            yield return new WaitForSeconds(1);
            CheckMatch(gridLevel[row1, col1], row1, col1);
            yield return new WaitForSeconds(1);
            CheckMatch(gridLevel[row2, col2], row2, col2);
        }
    }

    // Check if there is a match
    public bool IsAMatch(GameObject element, int row, int col)
    {
        bool res = false;

        bool sameColor = true;
        int pos = 1, nSameColor = 1;
        Material elementMaterial = element.GetComponent<Renderer>().sharedMaterial;

        // Check horizontal
        while (sameColor)
        {
            if (IsOnLevel(row - pos, col))
            {
                if (elementMaterial == gridLevel[row - pos, col].GetComponent<Renderer>().sharedMaterial)
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
                if (elementMaterial == gridLevel[row + pos, col].GetComponent<Renderer>().sharedMaterial)
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
                if (elementMaterial == gridLevel[row, col - pos].GetComponent<Renderer>().sharedMaterial)
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
                if (elementMaterial == gridLevel[row, col + pos].GetComponent<Renderer>().sharedMaterial)
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
    private bool CheckMatch(GameObject element, int row, int col)
    {
        bool res = false;

        List<GameObject> sameColorVertical = new List<GameObject>();
        List<GameObject> sameColorHorizontal = new List<GameObject>();

        sameColorVertical.Add(element);
        sameColorHorizontal.Add(element);

        bool sameColor = true;
        int pos = 1;
        Material elementMaterial = element.GetComponent<Renderer>().sharedMaterial;

        // Check horizontal
        while (sameColor)
        {
            if(IsOnLevel(row - pos, col))
            {
                if (elementMaterial == gridLevel[row - pos, col].GetComponent<Renderer>().sharedMaterial)
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
                if (elementMaterial == gridLevel[row + pos, col].GetComponent<Renderer>().sharedMaterial)
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
                if (elementMaterial == gridLevel[row, col - pos].GetComponent<Renderer>().sharedMaterial)
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
                if (elementMaterial == gridLevel[row, col + pos].GetComponent<Renderer>().sharedMaterial)
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

        if(sameColorHorizontal.Count >= 3 && sameColorVertical.Count >= 3)
        {
            for (int i = sameColorHorizontal.Count - 1; i >= 0; i--)
            {
                gridLevel[(int)sameColorHorizontal[i].transform.position.x, (int)sameColorHorizontal[i].transform.position.y] = null;
                Destroy(sameColorHorizontal[i]);
            }

            for (int i = sameColorVertical.Count - 1; i > 0; i--)
            {
                gridLevel[(int)sameColorVertical[i].transform.position.x, (int)sameColorVertical[i].transform.position.y] = null;
                Destroy(sameColorVertical[i]);
            }

            res = true;
            MoveDownPieces();
        }
        else if(sameColorHorizontal.Count >= 3)
        {
            Debug.Log("Horizontal de " + sameColorHorizontal.Count);
            for(int i = sameColorHorizontal.Count-1; i >= 0; i--)
            {
                gridLevel[(int)sameColorHorizontal[i].transform.position.x, (int)sameColorHorizontal[i].transform.position.y] = null;
                Destroy(sameColorHorizontal[i]);
            }

            res = true;
            MoveDownPieces();
        }
        else if (sameColorVertical.Count >= 3)
        {
            Debug.Log("Vertical de " + sameColorVertical.Count);
            for (int i = sameColorVertical.Count - 1; i >= 0; i--)
            {
                gridLevel[(int)sameColorVertical[i].transform.position.x, (int)sameColorVertical[i].transform.position.y] = null;
                Destroy(sameColorVertical[i]);
            }

            res = true;
            MoveDownPieces();
        }

        return res;
    }

    public bool IsOnLevel(int row, int col)
    {
        if(row < gridLevel.GetLength(0) && col < gridLevel.GetLength(1) && row >= 0 && col >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoveDownPieces()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(gridLevel[x,y] == null)
                {
                    for(int yAux = y + 1; yAux < height; yAux++)
                    {
                        if(gridLevel[x, yAux] != null)
                        {
                            gridLevel[x, y] = gridLevel[x, yAux];
                            gridLevel[x, yAux] = null;
                            gridLevel[x, y].transform.position = new Vector2(x, y);
                            break;
                        }
                    }
                }
            }
        }

        StartCoroutine(FillBlanks());
    }

    private IEnumerator FillBlanks()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gridLevel[x, y] == null)
                {
                    gridLevel[x, y] = Instantiate(levelElemnts[UnityEngine.Random.Range(0, levelElemnts.Length - 1)], new Vector2(x, y), Quaternion.identity);
                }
            }
        }

        for (int i = 0; i < gridLevel.GetLength(0); i++)
        {
            for (int j = 0; j < gridLevel.GetLength(1); j++)
            {
                while (IsAMatch(gridLevel[i, j], i, j))
                {
                    yield return new WaitForSeconds(2);
                    CheckMatch(gridLevel[i, j], i, j);
                }
            }
        }
    }
}
