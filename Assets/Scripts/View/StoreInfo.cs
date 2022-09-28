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
    private IIAPGameService iapService;

    public Button _buyAdGemsButton;
    [SerializeField]
    private TMP_Text _iapGemsCostText = null;

    public int priceHorizontalBooster, priceVerticalBooster, priceBombBooster, priceColorBombBooster, coinsPerAd;

    public TMPro.TextMeshProUGUI horizontalBoosters, verticalBoosters, bombBoosters, colorBombBoosters, coins;
    public TMPro.TextMeshProUGUI textPriceHorizontalBooster, textPriceVerticalBooster, textPriceBombBooster, textPriceColorBombBooster, textGoldPerAd;
    public TMPro.TextMeshProUGUI textPriceOrangeHudColor, textPriceBlueHudColor, textPriceGreenHudColor, textPricePurpleHudColor;
    public TMPro.TextMeshProUGUI textGoldPerBuy, textPriceGold;
    public TMPro.TextMeshProUGUI textItemsBowtie, textItemsTopHat, textItemsFrogHat, textItemsLogoPin, textItemsLogoHat;

    private void Awake()
    {
        gameConfig = ServiceLocator.GetService<GameConfigService>();
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        adsService = ServiceLocator.GetService<AdsGameService>();
        analytics = ServiceLocator.GetService<AnalyticsGameService>();
        iapService = ServiceLocator.GetService<IIAPGameService>();
    }

    // Start is called before the first frame update
    void Start()
    {
        textGoldPerAd.text = gameConfig.GoldPerAd.ToString();
        textGoldPerBuy.text = gameConfig.GoldPerBuy.ToString();
        //textPriceGold.text += iapService.GetLocalizedPrice("test1");

        textPriceHorizontalBooster.text = gameConfig.PriceHorizontalLineBooster.ToString();
        textPriceVerticalBooster.text = gameConfig.PriceVericalLineBooster.ToString();
        textPriceBombBooster.text = gameConfig.PriceBombBooster.ToString();
        textPriceColorBombBooster.text = gameConfig.PriceColorBombBooster.ToString();
        textPriceOrangeHudColor.text = gameConfig.PriceHUDColors.ToString();
        textPriceBlueHudColor.text = gameConfig.PriceHUDColors.ToString();
        textPriceGreenHudColor.text = gameConfig.PriceHUDColors.ToString();
        textPricePurpleHudColor.text = gameConfig.PriceHUDColors.ToString();

        UpdateInfo();
        StartCoroutine(WaitForIAPReady());
    }

    private void UpdateInfo()
    {
        horizontalBoosters.text = gameProgression.HorizontalLineBoosters.ToString();
        verticalBoosters.text = gameProgression.VerticalLineBoosters.ToString();
        bombBoosters.text = gameProgression.BombBoosters.ToString();
        colorBombBoosters.text = gameProgression.ColorBombBoosters.ToString();
        coins.text = gameProgression.Gold.ToString();

        textItemsBowtie.text = gameProgression.cosmeticsBought[1].ToString();
        textItemsFrogHat.text = gameProgression.cosmeticsBought[2].ToString();
        textItemsLogoHat.text = gameProgression.cosmeticsBought[4].ToString();
        textItemsLogoPin.text = gameProgression.cosmeticsBought[3].ToString();
        textItemsTopHat.text = gameProgression.cosmeticsBought[5].ToString();
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
            gameProgression.UpdateGold(-gameConfig.PriceHUDColors);
            gameProgression.UpdateHUDColor(HUDColors.ORANGE);
        }
    }

    public void BuyBlueHUD()
    {
        if (gameProgression.Gold >= gameConfig.PriceHUDColors)
        {
            gameProgression.UpdateGold(-gameConfig.PriceHUDColors);
            gameProgression.UpdateHUDColor(HUDColors.BLUE);
        }
    }

    public void BuyGreenHUD()
    {
        if (gameProgression.Gold >= gameConfig.PriceHUDColors)
        {
            gameProgression.UpdateGold(-gameConfig.PriceHUDColors);
            gameProgression.UpdateHUDColor(HUDColors.GREEN);
        }
    }

    public void BuyPurpleHUD()
    {
        if (gameProgression.Gold >= gameConfig.PriceHUDColors)
        {
            gameProgression.UpdateGold(-gameConfig.PriceHUDColors);
            gameProgression.UpdateHUDColor(HUDColors.PURPLE);
        }
    }

    public void BuyCosmetic(int id)
    {
        if(gameProgression.Gold >= 25)  // CHANGE TO REMOTE CONFIG
        {
            gameProgression.UpdateGold(-25);
            gameProgression.UpdateCosmetics(id, 1);
            UpdateInfo();
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

    public async void PurchaseIAPGems()
    {
        if (await iapService.StartPurchase("test1"))
        {
            gameProgression.UpdateGold(gameConfig.GoldPerBuy);
        }
        else
        {
            Debug.LogError("Purchase failed");
        }
    }

    IEnumerator WaitForIAPReady()
    {
        textPriceGold.text = "Loading...";
        //_buyIAPGemsButton.interactable = false;
        while (!iapService.IsReady())
        {
            yield return new WaitForSeconds(0.5f);
        }

        //_buyIAPGemsButton.interactable = true;
        textPriceGold.text = iapService.GetLocalizedPrice("test1");
    }

}
