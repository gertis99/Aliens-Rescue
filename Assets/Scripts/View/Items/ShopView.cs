using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    private GameConfigService gameConfig;
    private GameProgressionService gameProgression;
    private AdsGameService adsService;
    private AnalyticsGameService analytics;
    private IIAPGameService iapService;
    private ShopController controller;
    private List<ShopItemView> items = new List<ShopItemView>();

    [SerializeField]
    private ShopItemView shopItemPrefab;
    [SerializeField]
    private Transform panel;

    [SerializeField]
    private TMP_Text gold;
    [SerializeField]
    private TMP_Text goldPerAd, goldPerBuy, priceGold;

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
        controller = new ShopController(gameProgression);

        while (panel.childCount > 0)
        {
            Transform child = panel.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (ShopItemModel shopItemModel in gameConfig.ShopItems)
        {
            items.Add(Instantiate(shopItemPrefab, panel));
            items[items.Count - 1].SetData(shopItemModel, OnPurchaseItem);
        }

        goldPerAd.text = gameConfig.GoldPerAd.ToString();
        goldPerBuy.text = gameConfig.GoldPerBuy.ToString();
        UpdateInfo();
        StartCoroutine(WaitForIAPReady());
    }

    private void UpdateInfo()
    {
        gold.text = gameProgression.GetCurrency("Gold").ToString();
        foreach(ShopItemView item in items)
        {
            item.UpdateVisuals();
        }
    }

    private void OnPurchaseItem(ShopItemModel model)
    {
        controller.PurchaseItem(model);
        UpdateInfo();
    }

    public async void PlayAd()
    {
        if (await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            gameProgression.UpdateCurrency("Gold", gameConfig.GoldPerAd);
            UpdateInfo();
        }
    }

    public async void PurchaseIAPGold()
    {
        if (await iapService.StartPurchase("test1"))
        {
            gameProgression.UpdateCurrency("Gold", gameConfig.GoldPerBuy);
            UpdateInfo();
        }
        else
        {
            Debug.LogError("Purchase failed");
        }
    }

    IEnumerator WaitForIAPReady()
    {
        priceGold.text = "Loading...";
        while (!iapService.IsReady())
        {
            yield return new WaitForSeconds(0.5f);
        }

        priceGold.text = iapService.GetLocalizedPrice("test1");
    }
}
