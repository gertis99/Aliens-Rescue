using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapManager : MonoBehaviour
{

    public GameObject elementSelected = null;
    public LevelManager levelManager;

    private void Update()
    {
        if (elementSelected)
        {
            CheckTryToMove();
        }
    }

    private void CheckTryToMove()
    {
        if(elementSelected.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x <= -0.5) // Derecha
        {
            //Debug.Log("Derecha");
            if(levelManager.IsOnLevel((int)elementSelected.transform.position.x + 1, (int)elementSelected.transform.position.y))
            {
                StartCoroutine(levelManager.SwapPositions((int)elementSelected.transform.position.x, (int)elementSelected.transform.position.y, (int)elementSelected.transform.position.x + 1, (int)elementSelected.transform.position.y));
                
            }
            elementSelected = null;

        }
        else if(elementSelected.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= 0.5)  // Izquierda
        {
            //Debug.Log("Izquierda");
            if (levelManager.IsOnLevel((int)elementSelected.transform.position.x - 1, (int)elementSelected.transform.position.y))
            {
                StartCoroutine(levelManager.SwapPositions((int)elementSelected.transform.position.x, (int)elementSelected.transform.position.y, (int)elementSelected.transform.position.x - 1, (int)elementSelected.transform.position.y));
                
            }
            elementSelected = null;

        }
        else if (elementSelected.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= -0.5) // Arriba
        {
            //Debug.Log("Arriba");
            if (levelManager.IsOnLevel((int)elementSelected.transform.position.x, (int)elementSelected.transform.position.y + 1))
            {
                StartCoroutine( levelManager.SwapPositions((int)elementSelected.transform.position.x, (int)elementSelected.transform.position.y, (int)elementSelected.transform.position.x, (int)elementSelected.transform.position.y + 1));
                
            }
            elementSelected = null;

        }
        else if (elementSelected.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y >= 0.5)  // Abajo
        {
            //Debug.Log("Abajo");
            if (levelManager.IsOnLevel((int)elementSelected.transform.position.x, (int)elementSelected.transform.position.y - 1))
            {
                StartCoroutine( levelManager.SwapPositions((int)elementSelected.transform.position.x, (int)elementSelected.transform.position.y, (int)elementSelected.transform.position.x, (int)elementSelected.transform.position.y - 1));
                
            }
            elementSelected = null;

        }
    }

    public void SetElementSelected(GameObject el)
    {
        elementSelected = el;
    }
}
