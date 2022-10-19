using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    [SerializeField]
    private ActiveBoosterHorizontalLineView horizotntalBooster;
    [SerializeField]
    private ActiveBoosterVerticalLineView verticalBooster;
    [SerializeField]
    private ActiveBoosterBombView bombBooster;
    [SerializeField]
    private ActiveBoosterColorBombView colorBombBooster;

    private GameProgressionService gameProgression;
    private GameConfigService gameConfig;

    private string currentHUDColor;
    [SerializeField]
    private Image HUD;

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
                Addressables.LoadAssetAsync<Sprite>(colorHud.ConsoleImage).Completed += handler =>
                {
                    HUD.sprite = handler.Result;
                };

                break;
            }
        }
    }

    void Start()
    {
        
    }

}
