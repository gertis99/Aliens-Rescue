using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreInfo : MonoBehaviour
{
    private GameConfigService gameConfig;
    //private GameProgressionService _gameProgression;
    private AdsGameService adsService;
    private AnalyticsGameService analytics;

    public int priceHorizontalBooster, priceVerticalBooster, priceBombBooster, priceColorBombBooster;

    public TMPro.TextMeshProUGUI horizontalBoosters, verticalBoosters, bombBoosters, colorBombBoosters, coins;
    public TMPro.TextMeshProUGUI textPriceHorizontalBooster, textPriceVerticalBooster, textPriceBombBooster, textPriceColorBombBooster;

    private void Awake()
    {
        gameConfig = ServiceLocator.GetService<GameConfigService>();
        //gameProgression = ServiceLocator.GetService<GameProgressionService>();
        adsService = ServiceLocator.GetService<AdsGameService>();
        analytics = ServiceLocator.GetService<AnalyticsGameService>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        horizontalBoosters.text = PlayerInfo.HorizontalBoosters.ToString();
        verticalBoosters.text = PlayerInfo.VerticalBoosters.ToString();
        bombBoosters.text = PlayerInfo.BombBoosters.ToString();
        colorBombBoosters.text = PlayerInfo.ColorBombBoosters.ToString();
        coins.text = PlayerInfo.Coins.ToString();

        textPriceHorizontalBooster.text = gameConfig.PriceHorizontalLineBooster.ToString();
        textPriceVerticalBooster.text = gameConfig.PriceVericalLineBooster.ToString();
        textPriceBombBooster.text = gameConfig.PriceBombBooster.ToString();
        textPriceColorBombBooster.text = gameConfig.PriceColorBombBooster.ToString();
    }

    public void BuyHorizontalBooster()
    {
        if(PlayerInfo.Coins >= priceHorizontalBooster)
        {
            PlayerInfo.Coins -= priceHorizontalBooster;
            PlayerInfo.HorizontalBoosters++;
            UpdateInfo();
            analytics.SendEvent("buyHorizontalLineBooster");
        }
    }

    public void BuyVerticalBooster()
    {
        if (PlayerInfo.Coins >= priceVerticalBooster)
        {
            PlayerInfo.Coins -= priceVerticalBooster;
            PlayerInfo.VerticalBoosters++;
            UpdateInfo();
            analytics.SendEvent("buyVerticalLineBooster");
        }
    }

    public void BuyBombBooster()
    {
        if (PlayerInfo.Coins >= priceBombBooster)
        {
            PlayerInfo.Coins -= priceBombBooster;
            PlayerInfo.BombBoosters++;
            UpdateInfo();
            analytics.SendEvent("buyBombBooster");
        }
    }

    public void BuyColorBombBooster()
    {
        if (PlayerInfo.Coins >= priceColorBombBooster)
        {
            PlayerInfo.Coins -= priceColorBombBooster;
            PlayerInfo.ColorBombBoosters++;
            UpdateInfo();
            analytics.SendEvent("buyColorBombBooster");
        }
    }
}
