using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoadScene : MonoBehaviour
{

    public static event Action<int> OnLoadButtonClicked = delegate (int sceneIndex) { };


    public void LoadScene(int sceneIndex) {
        OnLoadButtonClicked(sceneIndex);
    }
}
