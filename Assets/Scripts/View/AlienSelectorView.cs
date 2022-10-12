using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlienSelectorView : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text nAliensText; 

    private int alienId;
    private Action<int> onClickedEvent;
    private GameProgressionService gameProgression;

    private void Awake()
    {
        gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    public void SetData(AlienInfo model, Action<int> onClickedEvent)
    {
        alienId = model.Id;
        image.sprite = Resources.Load<Sprite>(model.Image);
        this.onClickedEvent = onClickedEvent;

        foreach(AlienModel alien in gameProgression.AliensRescued)
        {
            if(alien.Id == alienId)
            {
                nAliensText.text = alien.Amount.ToString();
            }
        }
    }

    public void OnClicked()
    {
        onClickedEvent?.Invoke(alienId);
    }
}
