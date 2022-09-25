using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreInfo : MonoBehaviour
{
    private GameConfigService gameConfig;
    private GameProgressionService gameProgression;
    private AdsGameService adsService;
    private AnalyticsGameService analytics;

    public int priceHorizontalBooster, priceVerticalBooster, priceBombBooster, priceColorBombBooster, coinsPerAd;

    public TMPro.TextMeshProUGUI horizontalBoosters, verticalBoosters, bombBoosters, colorBombBoosters, coins;
    public TMPro.TextMeshProUGUI textPriceHorizontalBooster, textPriceVerticalBooster, textPriceBombBooster, textPriceColorBombBooster, textGoldPerAd;

    private void Awake()
    {
        gameConfig = ServiceLocator.GetService<GameConfigService>();
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        adsService = ServiceLocator.GetService<AdsGameService>();
        analytics = ServiceLocator.GetService<AnalyticsGameService>();
    }

    // Start is called before the first frame update
    void Start()
    {
        textGoldPerAd.text = gameConfig.GoldPerAd.ToString();
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        horizontalBoosters.text = gameProgression.HorizontalLineBoosters.ToString();
        verticalBoosters.text = gameProgression.VerticalLineBoosters.ToString();
        bombBoosters.text = gameProgression.BombBoosters.ToString();
        colorBombBoosters.text = gameProgression.ColorBombBoosters.ToString();
        coins.text = gameProgression.Gold.ToString();

        textPriceHorizontalBooster.text = gameConfig.PriceHorizontalLineBooster.ToString();
        textPriceVerticalBooster.text = gameConfig.PriceVericalLineBooster.ToString();
        textPriceBombBooster.text = gameConfig.PriceBombBooster.ToString();
        textPriceColorBombBooster.text = gameConfig.PriceColorBombBooster.ToString();
    }

    public void BuyHorizontalBooster()
    {
        if(gameProgression.Gold >= gameConfig.PriceHorizontalLineBooster)
        {
            gameProgression.UpdateGold(-gameConfig.PriceHorizontalLineBooster);
            gameProgression.UpdateHorizontalLineBoosters(1);
            UpdateInfo();
            analytics.SendEvent("buyHorizontalLineBooster");
        }
    }

    public void BuyVerticalBooster()
    {
        if (gameProgression.Gold >= gameConfig.PriceVericalLineBooster)
        {
            gameProgression.UpdateGold(-gameConfig.PriceVericalLineBooster);
            gameProgression.UpdateVerticalLineBoosters(1);
            UpdateInfo();
            analytics.SendEvent("buyVerticalLineBooster");
        }
    }

    public void BuyBombBooster()
    {
        if (gameProgression.Gold >= gameConfig.PriceBombBooster)
        {
            gameProgression.UpdateGold(-gameConfig.PriceBombBooster);
            gameProgression.UpdateBombBoosters(1);
            UpdateInfo();
            analytics.SendEvent("buyBombBooster");
        }
    }

    public void BuyColorBombBooster()
    {
        if (gameProgression.Gold >= gameConfig.PriceColorBombBooster)
        {
            gameProgression.UpdateGold(-gameConfig.PriceColorBombBooster);
            gameProgression.UpdateColorBombBoosters(1);
            UpdateInfo();
            analytics.SendEvent("buyColorBombBooster");
        }
    }

    public async void PlayAd()
    {
        if (await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            gameProgression.UpdateGold(gameConfig.GoldPerAd);
            UpdateInfo();
        }
    }
}
