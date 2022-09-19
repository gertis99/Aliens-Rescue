using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuInfo : MonoBehaviour
{
    public TMPro.TextMeshProUGUI currentLevel;
    private GameProgressionService gameProgression;

    private void Awake()
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel.text = gameProgression.CurrentLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
