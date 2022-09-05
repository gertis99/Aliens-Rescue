using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorInfo : MonoBehaviour
{

    public List<Button> buttons;

    
    void Start()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            if (i + 1 > PlayerInfo.ActualLevel)
                buttons[i].interactable = false;
        }    
    }

    
    void Update()
    {
        
    }
}
