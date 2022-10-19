using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TermsPanel : MonoBehaviour
{
    public void Accept()
    {
        PlayerPrefs.SetInt("termsAccepted", 1);
        Destroy(this.gameObject);
    }

    public void OpenUrl()
    {
        Application.OpenURL("https://sites.google.com/view/privacypolicygertis/inicio");
    }

    public void Decline()
    {
        Application.Quit();
    }
}
