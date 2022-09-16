using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Element
{

    private AlienType alienType;

    public Alien(int x, int y, AlienType aType) 
    {
        posX = x;
        posY = y;
        alienType = aType;
    }

    public AlienType GetElementType()
    {
        return alienType;
    }
}
