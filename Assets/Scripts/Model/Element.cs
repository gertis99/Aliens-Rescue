using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class containing the info of each element in the grid
 * */

public class Element
{

    [SerializeField]
    private int posX, posY;
    private int colorType; // 0 red 1 blue 2 green 3 orange 4 purple 5 yellow

    // Contructor
    public Element(int x, int y, int type)
    {
        posX = x;
        posY = y;
        colorType = type;
    }

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

    public int GetColorType()
    {
        return colorType;
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

    public void SetColorType(int type)
    {
        colorType = type;
    }
}
