using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticPrefab : MonoBehaviour
{
    public Image image;
    public Image selected;
    public bool isSelected;
    public int Id { get; set; }

    public void SetSprite(Sprite spr)
    {
        image.sprite = spr;
    }
}
