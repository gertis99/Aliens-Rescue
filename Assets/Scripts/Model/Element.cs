using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class containing the info of each element in the grid
 * */

public class Element
{

    [SerializeField]
    protected int posX, posY;

    /***************************************************************************/
    // Getters
    public int GetPosX()
    {
        return posX;
    }

    public int GetPosY()
    {
        return posY;
    }

    /***************************************************************************/
    // Setters
    public void SetPosX(int x)
    {
        posX = x;
    }

    public void SetPosY(int y)
    {
        posY = y;
    }

    public void SetPos(int x, int y)
    {
        posX = x;
        posY = y;
    }
}
