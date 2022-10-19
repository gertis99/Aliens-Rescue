using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoadAlienCosmetic : MonoBehaviour
{
    public static event Action<int> OnLoadAlienCosmeticButtonClicked = delegate (int alienId) { };

    public void LoadAlienCosmeticScene(int alienId)
    {
        OnLoadAlienCosmeticButtonClicked(alienId);
    }
}
