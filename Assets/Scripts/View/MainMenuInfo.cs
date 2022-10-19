using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuInfo : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI currentLevel;
    [SerializeField] private GameObject termPanelPrefab;
    private GameProgressionService gameProgression;
    private bool termsReaded => PlayerPrefs.GetInt("termsAccepted", 0) == 1;

    private void Awake()
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel.text = gameProgression.CurrentLevel.ToString();
        if (!termsReaded)
            Instantiate(termPanelPrefab, this.gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
