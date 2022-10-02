using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    private GameConfigService gameConfig;
    private GameProgressionService gameProgression;
    private AdsGameService adsService;
    private AnalyticsGameService analytics;
    private IIAPGameService iapService;
    private ShopController controller;

    public ShopItemView shopItemPrefab;
    public Transform panel;

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
            Instantiate(shopItemPrefab, panel).SetData(shopItemModel, OnPurchaseItem);
        }
    }

    private void OnPurchaseItem(ShopItemModel model)
    {
        controller.PurchaseItem(model);
    }

}
