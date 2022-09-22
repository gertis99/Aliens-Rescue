using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreInfo : MonoBehaviour
{
    private GameConfigService gameConfig;
    private GameProgressionService gameProgression;
    private AdsGameService adsService;
    private AnalyticsGameService analytics;
    private IIAPGameService _iapService;

    public Button _buyAdGemsButton;
    [SerializeField]
    private TMP_Text _iapGemsCostText = null;

    public int priceHorizontalBooster, priceVerticalBooster, priceBombBooster, priceColorBombBooster, coinsPerAd;

    public TMPro.TextMeshProUGUI horizontalBoosters, verticalBoosters, bombBoosters, colorBombBoosters, coins;
    public TMPro.TextMeshProUGUI textPriceHorizontalBooster, textPriceVerticalBooster, textPriceBombBooster, textPriceColorBombBooster, textGoldPerAd;
    public TMPro.TextMeshProUGUI textPriceOrangeHudColor, textPriceBlueHudColor, textPriceGreenHudColor, textPricePurpleHudColor;

    private void Awake()
    {
        gameConfig = ServiceLocator.GetService<GameConfigService>();
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        adsService = ServiceLocator.GetService<AdsGameService>();
        analytics = ServiceLocator.GetService<AnalyticsGameService>();
        _iapService = ServiceLocator.GetService<IIAPGameService>();
    }

    // Start is called before the first frame update
    void Start()
    {
        textGoldPerAd.text = gameConfig.GoldPerAd.ToString();

        textPriceHorizontalBooster.text = gameConfig.PriceHorizontalLineBooster.ToString();
        textPriceVerticalBooster.text = gameConfig.PriceVericalLineBooster.ToString();
        textPriceBombBooster.text = gameConfig.PriceBombBooster.ToString();
        textPriceColorBombBooster.text = gameConfig.PriceColorBombBooster.ToString();
        textPriceOrangeHudColor.text = gameConfig.PriceHUDColors.ToString();
        textPriceBlueHudColor.text = gameConfig.PriceHUDColors.ToString();
        textPriceGreenHudColor.text = gameConfig.PriceHUDColors.ToString();
        textPricePurpleHudColor.text = gameConfig.PriceHUDColors.ToString();

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        horizontalBoosters.text = gameProgression.HorizontalLineBoosters.ToString();
        verticalBoosters.text = gameProgression.VerticalLineBoosters.ToString();
        bombBoosters.text = gameProgression.BombBoosters.ToString();
        colorBombBoosters.text = gameProgression.ColorBombBoosters.ToString();
        coins.text = gameProgression.Gold.ToString();
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

    public void BuyOrangeHUD()
    {
        if(gameProgression.Gold >= gameConfig.PriceHUDColors)
        {
            gameProgression.UpdateHUDColor(HUDColors.ORANGE);
        }
    }

    public void BuyBlueHUD()
    {
        if (gameProgression.Gold >= gameConfig.PriceHUDColors)
        {
            gameProgression.UpdateHUDColor(HUDColors.BLUE);
        }
    }

    public void BuyGreenHUD()
    {
        if (gameProgression.Gold >= gameConfig.PriceHUDColors)
        {
            gameProgression.UpdateHUDColor(HUDColors.GREEN);
        }
    }

    public void BuyPurpleHUD()
    {
        if (gameProgression.Gold >= gameConfig.PriceHUDColors)
        {
            gameProgression.UpdateHUDColor(HUDColors.PURPLE);
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
