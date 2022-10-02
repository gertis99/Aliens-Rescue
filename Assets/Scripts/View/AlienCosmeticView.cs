using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienCosmeticView : MonoBehaviour
{
    public Image currentAlien;
    public static Sprite currentSprite;
    public Cosmetic[] cosmetics;
    public GameObject panel;
    public GameObject cosmeticPrefab;
    private GameObject[] buttons;

    private GameProgressionService gameProgressionService;
    private GameConfigService gameConfigService;

    private void Awake()
    {
        gameProgressionService = ServiceLocator.GetService<GameProgressionService>();
        gameConfigService = ServiceLocator.GetService<GameConfigService>();
    }

    private void Start()
    {
        currentAlien.sprite = currentSprite;
        buttons = new GameObject[cosmetics.Length];

        for (int i = 0; i < gameProgressionService.Cosmetics.Count; i++)
        {
            GameObject cosAux = Instantiate(cosmeticPrefab, panel.transform);

            foreach(CosmeticItemInfo cosmeticInfo in gameConfigService.Cosmetics)
            {
                if(cosmeticInfo.CosmeticName == gameProgressionService.Cosmetics[i].Name)
                {
                    cosAux.GetComponent<CosmeticPrefab>().SetSprite(Resources.Load<Sprite>(cosmeticInfo.Image));
                    //cosAux.GetComponent<CosmeticPrefab>().Id = cosmetics[i].id;
                    //buttons[i] = cosAux;
                    break;
                }
            }

            /*if (gameProgressionService.cosmeticsBought[cosAux.GetComponent<CosmeticPrefab>().Id] > 0)
                buttons[i].GetComponent<Button>().interactable = true;
            else
                buttons[i].GetComponent<Button>().interactable = false;*/
        }
    }


}
