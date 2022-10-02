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

    private HUDColors currentHUDColor;
    public Image HUD;
    public Sprite[] huds;

    private void Awake()
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        horizotntalBooster.boostersLeft = gameProgression.GetActiveBoosterAmount("HorizontalLineBooster");
        verticalBooster.boostersLeft = gameProgression.GetActiveBoosterAmount("VerticalLineBooster");
        bombBooster.boostersLeft = gameProgression.GetActiveBoosterAmount("BombBooster");
        colorBombBooster.boostersLeft = gameProgression.GetActiveBoosterAmount("ColorBombBooster");
        /*currentHUDColor = gameProgression.currentHUDColor;

        HUD.sprite = huds[(int)currentHUDColor];*/
    }

    void Start()
    {
        
    }

}
