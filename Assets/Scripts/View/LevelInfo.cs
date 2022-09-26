using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    public ActiveBoosterHorizontalLineView horizotntalBooster;
    public ActiveBoosterVerticalLineView verticalBooster;
    public ActiveBoosterBombView bombBooster;
    public ActiveBoosterColorBombView colorBombBooster;

    private GameProgressionService gameProgression;

    private HUDColors currentHUDColor;
    public Image HUD;
    public Sprite[] huds;

    private void Awake()
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        horizotntalBooster.boostersLeft = gameProgression.HorizontalLineBoosters;
        verticalBooster.boostersLeft = gameProgression.VerticalLineBoosters;
        bombBooster.boostersLeft = gameProgression.BombBoosters;
        colorBombBooster.boostersLeft = gameProgression.ColorBombBoosters;
        currentHUDColor = gameProgression.currentHUDColor;

        HUD.sprite = huds[(int)currentHUDColor];
    }

    void Start()
    {
        
    }

}
