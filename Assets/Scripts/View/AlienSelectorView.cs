using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienSelectorView : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private int alienId;
    private Action<int> onClickedEvent;

    private void Start()
    {
        
    }

    public void SetData(AlienInfo model, Action<int> onClickedEvent)
    {
        alienId = model.Id;
        image.sprite = Resources.Load<Sprite>(model.Image);
        this.onClickedEvent = onClickedEvent;
    }

    public void OnClicked()
    {
        onClickedEvent?.Invoke(alienId);
    }
}
