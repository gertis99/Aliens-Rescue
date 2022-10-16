using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AlienCosmeticView : MonoBehaviour
{
    public Image currentAlienImage;
    public int currentAlienId;
    public Cosmetic[] cosmetics;
    public Transform panel;
    public CosmeticPrefab cosmeticPrefab;

    private AlienCosmeticController controller;

    private GameProgressionService gameProgressionService;
    private GameConfigService gameConfigService;

    private List<CosmeticPrefab> cosmeticsPrefab = new List<CosmeticPrefab>();

    private void Awake()
    {
        gameProgressionService = ServiceLocator.GetService<GameProgressionService>();
        gameConfigService = ServiceLocator.GetService<GameConfigService>();
    }

    private void Start()
    {
        currentAlienId = PlayerPrefs.GetInt("AlienToLoad", 1);

        controller = new AlienCosmeticController();

        foreach(CosmeticItemModel cosmetic in gameProgressionService.Cosmetics)
        {
            CosmeticPrefab cosAux = Instantiate(cosmeticPrefab, panel);
            cosAux.SetData(cosmetic, currentAlienId, OnCosmeticClicked);
            cosmeticsPrefab.Add(cosAux);
        }

        foreach (CosmeticItemModel cosmetic in gameProgressionService.Cosmetics)
        {
            if (cosmetic.SelectedAliensIds.Contains(currentAlienId)){
                Addressables.LoadAssetAsync<Sprite>(gameConfigService.GetAlienInfo(currentAlienId).Image + "_" + cosmetic.Name).Completed += handler =>
                {
                    currentAlienImage.sprite = handler.Result;
                };

                //currentAlienImage.sprite = Resources.Load<Sprite>(gameConfigService.GetAlienInfo(currentAlienId).Image + "_" + cosmetic.Name);
                return;
            }
        }

        foreach (AlienInfo alienInfo in gameConfigService.Aliens)
        {
            if (alienInfo.Id == currentAlienId)
            {
                Addressables.LoadAssetAsync<Sprite>(alienInfo.Image).Completed += handler =>
                {
                    currentAlienImage.sprite = handler.Result;
                };

                //currentAlienImage.sprite = Resources.Load<Sprite>(alienInfo.Image);
                break;
            }
        }
    }

    private void OnCosmeticClicked(string cosmeticName, int alienId)
    {
        controller.SelectedCosmetic(cosmeticName, alienId);

        foreach(CosmeticPrefab cosmetic in cosmeticsPrefab)
        {
            cosmetic.UpdateVisuals();
        }

        foreach (CosmeticItemModel cosmetic in gameProgressionService.Cosmetics)
        {
            if (cosmetic.SelectedAliensIds.Contains(currentAlienId))
            {
                Addressables.LoadAssetAsync<Sprite>(gameConfigService.GetAlienInfo(currentAlienId).Image + "_" + cosmetic.Name).Completed += handler =>
                {
                    currentAlienImage.sprite = handler.Result;
                };

                //currentAlienImage.sprite = Resources.Load<Sprite>(gameConfigService.GetAlienInfo(currentAlienId).Image + "_" + cosmetic.Name);
                return;
            }
        }

        foreach (AlienInfo alienInfo in gameConfigService.Aliens)
        {
            if (alienInfo.Id == currentAlienId)
            {
                Addressables.LoadAssetAsync<Sprite>(alienInfo.Image).Completed += handler =>
                {
                    currentAlienImage.sprite = handler.Result;
                };

                //currentAlienImage.sprite = Resources.Load<Sprite>(alienInfo.Image);
                break;
            }
        }
    }
}
