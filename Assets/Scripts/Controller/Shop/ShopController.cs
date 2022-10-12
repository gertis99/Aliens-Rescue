using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController
{
    private GameProgressionService gameProgression;
    private AnalyticsGameService analytics;

    public ShopController(GameProgressionService gameProgr)
    {
        gameProgression = gameProgr;
        analytics = ServiceLocator.GetService<AnalyticsGameService>();
    }

    public void PurchaseItem(ShopItemModel item)
    {
        analytics.SendEvent("buyShopItem", new Dictionary<string, object> { ["shopItemName"] = item.ItemName});

        if(item.Type == "Cosmetic")
        {
            gameProgression.UpdateCosmetic(item.ItemName, item.Amount, item.Amount);
            gameProgression.UpdateCurrency(item.Price.Name, -item.Price.Amount);
            return;
        }

        if (item.Type == "ActiveBooster")
        {
            gameProgression.UpdateActiveBooster(item.ItemName, item.Amount);
            gameProgression.UpdateCurrency(item.Price.Name, -item.Price.Amount);
            return;
        }

        if (item.Type == "HudColor")
        {
            gameProgression.UpdateColorHud(item.ItemName, true);
            gameProgression.UpdateCurrency(item.Price.Name, -item.Price.Amount);
            gameProgression.currentHudColor = item.ItemName;
            return;
        }
    }
}
