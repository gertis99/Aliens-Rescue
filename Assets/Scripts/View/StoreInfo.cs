using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreInfo : MonoBehaviour
{
    public int priceHorizontalBooster, priceVerticalBooster, priceBombBooster, priceColorBombBooster;

    public TMPro.TextMeshProUGUI horizontalBoosters, verticalBoosters, bombBoosters, colorBombBoosters, coins;
    public TMPro.TextMeshProUGUI textPriceHorizontalBooster, textPriceVerticalBooster, textPriceBombBooster, textPriceColorBombBooster;

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

        textPriceHorizontalBooster.text = priceHorizontalBooster.ToString();
        textPriceVerticalBooster.text = priceVerticalBooster.ToString();
        textPriceBombBooster.text = priceBombBooster.ToString();
        textPriceColorBombBooster.text = priceColorBombBooster.ToString();
    }

    public void BuyHorizontalBooster()
    {
        if(PlayerInfo.Coins >= priceHorizontalBooster)
        {
            PlayerInfo.Coins -= priceHorizontalBooster;
            PlayerInfo.HorizontalBoosters++;
            UpdateInfo();
        }
    }

    public void BuyVerticalBooster()
    {
        if (PlayerInfo.Coins >= priceVerticalBooster)
        {
            PlayerInfo.Coins -= priceVerticalBooster;
            PlayerInfo.VerticalBoosters++;
            UpdateInfo();
        }
    }

    public void BuyBombBooster()
    {
        if (PlayerInfo.Coins >= priceBombBooster)
        {
            PlayerInfo.Coins -= priceBombBooster;
            PlayerInfo.BombBoosters++;
            UpdateInfo();
        }
    }

    public void BuyColorBombBooster()
    {
        if (PlayerInfo.Coins >= priceColorBombBooster)
        {
            PlayerInfo.Coins -= priceColorBombBooster;
            PlayerInfo.ColorBombBoosters++;
            UpdateInfo();
        }
    }
}
