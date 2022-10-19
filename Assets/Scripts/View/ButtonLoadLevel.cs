using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoadLevel : MonoBehaviour
{
    public static event Action<int> OnLoadLevelButtonClicked = delegate (int levelId) { };

    public void LoadLevelScene(int levelId)
    {
        OnLoadLevelButtonClicked(levelId);
    }
}
