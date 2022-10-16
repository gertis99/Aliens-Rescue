using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text description, cost, amount;
    [SerializeField]
    private GameObject selected;

    private ShopItemModel model;
    private Action<ShopItemModel> onClickedEvent;

    private GameProgressionService gameProgression;

    public void SetData(ShopItemModel model, Action<ShopItemModel> onClickedEvent)
    {
        this.model = model;
        this.onClickedEvent = onClickedEvent;
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        selected.SetActive(false);
        UpdateVisuals();
    }

    private bool CanPay()
    {
        return gameProgression.GetCurrency(model.Price.Name) >= model.Price.Amount;
    }

    private void UpdateVisuals()
    {
        if (model == null)
            return;

        Addressables.LoadAssetAsync<Sprite>(model.Image).Completed += handler =>
        {
            image.sprite = handler.Result;
        };

        description.text = model.Description;
        cost.text = model.Price.Amount.ToString();
        
        if(model.Type == "Cosmetic")
        {
            amount.text = gameProgression.GetCosmeticAmount(model.ItemName).ToString();
            if (gameProgression.GetCosmeticAmount(model.ItemName) >= 6)
                selected.SetActive(true);
        }

        if(model.Type == "ActiveBooster")
        {
            amount.text = gameProgression.GetActiveBoosterAmount(model.ItemName).ToString();
        }

        if(model.Type == "HudColor")
        {
            amount.gameObject.SetActive(false);
            selected.SetActive(gameProgression.IsHudColorActive(model.ItemName));
        }
    }

    public void OnClicked()
    {
        if(model != null && !selected.activeInHierarchy && CanPay())
        {
            onClickedEvent?.Invoke(model);
            UpdateVisuals();
        }
    }
}
