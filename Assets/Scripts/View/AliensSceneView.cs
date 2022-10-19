using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliensSceneView : MonoBehaviour
{
    [SerializeField]
    private Transform panel;
    [SerializeField]
    private AlienSelectorView alienButtonPrefab;

    private GameConfigService gameConfig;

    private void Awake()
    {
        gameConfig = ServiceLocator.GetService<GameConfigService>();
    }

    private void Start()
    {
        while (panel.childCount > 0)
        {
            Transform child = panel.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (AlienInfo alienModel in gameConfig.Aliens)
        {
            Instantiate(alienButtonPrefab, panel).SetData(alienModel, OnClickedAlien);
        }
    }

    private void OnClickedAlien(int id)
    {
        PlayerPrefs.SetInt("AlienToLoad", id);
    }
}
