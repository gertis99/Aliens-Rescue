using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Element
{

    //private AlienType alienType;
    public int AlienId { get; private set; }

    public Alien(int x, int y, int alienId) 
    {
        posX = x;
        posY = y;
        AlienId = alienId;
    }

    /*public AlienType GetElementType()
    {
        return alienType;
    }*/
}
