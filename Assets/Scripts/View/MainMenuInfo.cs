using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuInfo : MonoBehaviour
{
    public TMPro.TextMeshProUGUI actualLevel;

    // Start is called before the first frame update
    void Start()
    {
        actualLevel.text = PlayerInfo.ActualLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
