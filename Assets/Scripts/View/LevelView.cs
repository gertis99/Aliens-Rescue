using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    public ActiveBoosterHorizontalLineView horizotntalBooster;
    public ActiveBoosterVerticalLineView verticalBooster;
    public ActiveBoosterBombView bombBooster;
    public ActiveBoosterColorBombView colorBombBooster;

    private GameProgressionService gameProgression;
    private GameConfigService gameConfig;

    private string currentHUDColor;
    public Image HUD;

    private void Awake()
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        gameConfig = ServiceLocator.GetService<GameConfigService>();
        horizotntalBooster.boostersLeft = gameProgression.GetActiveBoosterAmount("HorizontalLineBooster");
        verticalBooster.boostersLeft = gameProgression.GetActiveBoosterAmount("VerticalLineBooster");
        bombBooster.boostersLeft = gameProgression.GetActiveBoosterAmount("BombBooster");
        colorBombBooster.boostersLeft = gameProgression.GetActiveBoosterAmount("ColorBombBooster");
        currentHUDColor = gameProgression.currentHudColor;

        foreach(ColorHudItemInfo colorHud in gameConfig.HudColors)
        {
            if(colorHud.ColorHudName == currentHUDColor)
            {
                HUD.sprite = Resources.Load<Sprite>(colorHud.ConsoleImage);
                break;
            }
        }
    }

    void Start()
    {
        
    }

}
