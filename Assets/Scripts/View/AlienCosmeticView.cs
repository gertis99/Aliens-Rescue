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

    private void Awake()
    {
        gameProgressionService = ServiceLocator.GetService<GameProgressionService>();
    }

    private void Start()
    {
        currentAlien.sprite = currentSprite;
        buttons = new GameObject[cosmetics.Length];

        for (int i = 0; i < cosmetics.Length; i++)
        {
            GameObject cosAux = Instantiate(cosmeticPrefab, panel.transform);
            cosAux.GetComponent<CosmeticPrefab>().SetSprite(cosmetics[i].sprite);
            cosAux.GetComponent<CosmeticPrefab>().Id = cosmetics[i].id;
            buttons[i] = cosAux;

            if (gameProgressionService.cosmeticsBought[cosAux.GetComponent<CosmeticPrefab>().Id] > 0)
                buttons[i].GetComponent<Button>().interactable = true;
            else
                buttons[i].GetComponent<Button>().interactable = false;
        }
    }


}
