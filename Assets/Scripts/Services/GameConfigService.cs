
using System.Collections.Generic;

public class GameConfigService : IService
{
    public int InitialGold { get; private set; }
    public int InitialHorizontalLineBooster { get; private set; }
    public int InitialVerticalLineBooster { get; private set; }
    public int InitialBombBooster { get; private set; }
    public int InitialColorBombBooster { get; private set; }
    public int PriceHorizontalLineBooster { get; private set; }
    public int PriceVericalLineBooster { get; private set; }
    public int PriceBombBooster { get; private set; }
    public int PriceColorBombBooster { get; private set; }
    public int PriceHUDColors { get; private set; }
    public int GoldPerAd { get; private set; }
    public int GoldPerBuy { get; private set; }
    public List<ActiveBoosterItemInfo> ActiveBoosters { get; private set; }
    public List<CosmeticItemInfo> Cosmetics { get; private set; }
    public List<ColorHudItemInfo> HudColors { get; private set; }
    public List<ShopItemModel> ShopItems { get; private set; }


    public void Initialize(RemoteConfigGameService dataProvider)
    {
        InitialGold = dataProvider.Get("InitialGold", 200);
        InitialHorizontalLineBooster = dataProvider.Get("InitialHorizontalLineBooster", 5);
        InitialVerticalLineBooster = dataProvider.Get("InitialVerticalLineBooster", 5);
        InitialBombBooster = dataProvider.Get("InitialBombBooster", 5);
        InitialColorBombBooster = dataProvider.Get("InitialColorBombBooster", 5);
        PriceHorizontalLineBooster = dataProvider.Get("PriceHorizontalLineBooster", 10);
        PriceVericalLineBooster = dataProvider.Get("PriceVericalLineBooster", 10);
        PriceBombBooster = dataProvider.Get("PriceBombBooster", 20);
        PriceColorBombBooster = dataProvider.Get("PriceColorBombBooster", 40);
        PriceHUDColors = dataProvider.Get("PriceHUDColors", 10);
        GoldPerAd = dataProvider.Get("GoldPerAd", 50);
        GoldPerBuy = dataProvider.Get("GoldPerBuy", 500);

        ActiveBoosters = dataProvider.Get("ActiveBoosters", new List<ActiveBoosterItemInfo>());
        Cosmetics = dataProvider.Get("Cosmetics", new List<CosmeticItemInfo>());
        HudColors = dataProvider.Get("HudColors", new List<ColorHudItemInfo>());
        ShopItems = dataProvider.Get("ShopItems", new List<ShopItemModel>());
    }

    public void Clear()
    {
    }
}
