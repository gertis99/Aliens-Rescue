using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class CosmeticPrefab : MonoBehaviour
{
    public Image image;
    public GameObject selected;
    public bool isSelected;
    public string cosmeticName;
    private CosmeticItemModel model;
    private int alienId;
    private Action<string, int> onClickedEvent;

    private GameConfigService gameConfig;
    private GameProgressionService gameProgression;

    public void SetData(CosmeticItemModel model, int alienId, Action<string, int> onClickedEvent)
    {
        gameConfig = ServiceLocator.GetService<GameConfigService>();
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
        this.model = model;
        this.alienId = alienId;
        this.onClickedEvent = onClickedEvent;
        
        foreach(CosmeticItemInfo cosmetic in gameConfig.Cosmetics)
        {
            if(cosmetic.CosmeticName == model.Name)
            {
                Addressables.LoadAssetAsync<Sprite>(cosmetic.Image).Completed += handler =>
                {
                    image.sprite = handler.Result;
                };

                //image.sprite = Resources.Load<Sprite>(cosmetic.Image);
            }
        }

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        foreach(CosmeticItemModel cosmetic in gameProgression.Cosmetics)
        {
            if(cosmetic.Name == model.Name)
            {
                isSelected = cosmetic.SelectedAliensIds.Contains(alienId);
                selected.SetActive(isSelected);
            }
        }

        if(model.AmountAvailable <= 0 && !isSelected)
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void SetSprite(Sprite spr)
    {
        image.sprite = spr;
    }

    public void OnClicked()
    {
        onClickedEvent?.Invoke(model.Name, alienId);
        //gameProgression.Save();
    }
}
