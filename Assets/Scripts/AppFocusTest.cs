using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppFocusTest : MonoBehaviour
{
    public static event Action OnFocusLost = delegate () { };

    void Start() 
    {
        Application.focusChanged += OnApplicationFocusEvent;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("OnApplicationFocus " + hasFocus);
        if (!hasFocus)
            OnFocusLost();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("OnApplicationPause " + pauseStatus);
        if (pauseStatus)
            OnFocusLost();
    }

    private void OnApplicationFocusEvent(bool focused)
    {
        Debug.Log("OnApplicationFocusEvent " + focused);
        if (!focused)
            OnFocusLost();
    }
}
