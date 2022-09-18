using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorInfo : MonoBehaviour
{

    public List<Button> buttons;
    private GameProgressionService gameProgression;

    private void Awake()
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    void Start()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            if (i + 1 > gameProgression.CurrentLevel)
                buttons[i].interactable = false;
        }    
    }

    
    void Update()
    {
        
    }
}
