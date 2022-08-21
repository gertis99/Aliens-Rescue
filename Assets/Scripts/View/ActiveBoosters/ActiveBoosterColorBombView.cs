using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveBoosterColorBombView : ActiveBoosterView
{
    private void Update()
    {
        if (isActivated && boostersLeft > 0)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit2D hitData = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);
                if (hitData)
                {
                    controller.ActiveBoosterColorBomb(hitData.collider.gameObject.transform.position);
                    boostersLeft--;
                    PlayerInfo.ColorBombBoosters--;
                    GetComponentInChildren<TMPro.TextMeshProUGUI>().text = boostersLeft.ToString();
                    isActivated = false;
                }
            }
        }

    }
}
