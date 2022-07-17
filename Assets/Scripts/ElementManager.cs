using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{

    public SwapManager swapManager;
    public int posX, posY;

    private void Start()
    {
        swapManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SwapManager>();
    }

    private void OnMouseDrag()
    {
        swapManager.SetElementSelected(this.gameObject);
    }

    private void OnMouseUp()
    {
        swapManager.SetElementSelected(null);
    }

    public void SetPos(int x, int y)
    {
        posX = x;
        posY = y;
    }
}
