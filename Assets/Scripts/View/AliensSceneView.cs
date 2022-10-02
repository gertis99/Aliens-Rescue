using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliensSceneView : MonoBehaviour
{
    public AlienView[] aliens;

    public void ClickOnAlien(int id)
    {
        for(int i = 0; i < aliens.Length; i++)
        {
            if(aliens[i].id == id)
            {
                AlienCosmeticView.currentSprite= aliens[i].sprite;
                return;
            }
        }
    }
}
