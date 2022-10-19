using System.Collections.Generic;

public class GameConfigService : IService
{
    public int GoldPerAd { get; private set; }
    public int GoldPerBuy { get; private set; }
    public int BoardWidth { get; private set; }
    public int BoardHeight { get; private set; }
    public int BoardColors { get; private set; }
    public List<ActiveBoosterItemInfo> ActiveBoosters { get; private set; }
    public List<CosmeticItemInfo> Cosmetics { get; private set; }
    public List<ColorHudItemInfo> HudColors { get; private set; }
    public List<ShopItemModel> ShopItems { get; private set; }
    public List<InGameCurrency> InitialCurrencies { get; private set; }
    public List<ActiveBoosterItemModel> InitialActiveBoosters { get; private set; }
    public List<AlienInfo> Aliens { get; private set; }
    public List<LevelInfo> Levels { get; private set; }


    public void Initialize(RemoteConfigGameService dataProvider)
    {
        GoldPerAd = dataProvider.Get("GoldPerAd", 50);
        GoldPerBuy = dataProvider.Get("GoldPerBuy", 500);
        BoardWidth = dataProvider.Get("InitialBoardWidth", 8);
        BoardHeight = dataProvider.Get("InitialBoardHeight", 8);
        BoardColors = dataProvider.Get("InitialBoardColors", 6);

        ActiveBoosters = dataProvider.Get("ActiveBoosters", new List<ActiveBoosterItemInfo>());
        Cosmetics = dataProvider.Get("Cosmetics", new List<CosmeticItemInfo>());
        HudColors = dataProvider.Get("HudColors", new List<ColorHudItemInfo>());
        ShopItems = dataProvider.Get("ShopItems", new List<ShopItemModel>());
        Aliens = dataProvider.Get("Aliens", new List<AlienInfo>());
        Levels = dataProvider.Get("Levels", new List<LevelInfo>());

        InitialCurrencies = dataProvider.Get("InitialCurrencies", new List<InGameCurrency>());
        InitialActiveBoosters = dataProvider.Get("InitialActiveBoosters", new List<ActiveBoosterItemModel>());
    }

    public AlienInfo GetAlienInfo(int id)
    {
        foreach(AlienInfo alien in Aliens)
        {
            if (alien.Id == id)
                return alien;
        }

        return null;
    }

    public bool ExistLevel(int id)
    {
        foreach(LevelInfo level in Levels)
        {
            if (level.Id == id)
                return true;
        }

        return false;
    }
    public void Clear()
    {
    }
}
