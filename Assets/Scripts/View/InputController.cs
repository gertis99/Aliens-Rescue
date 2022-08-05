using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manage the inputs in the screen
 */

public class InputController : MonoBehaviour
{
    /*public delegate void ElementSelected(GameObject element);
    public delegate void ElementMoved(Vector2 pos);
    public static event ElementSelected OnElementSelected;
    public static event ElementMoved OnElementMoved;

    private GameObject elementSelected = null;

    private void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (Input.GetMouseButton(0))
        {
            if (elementSelected == null)
            {
                RaycastHit2D hitData = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);
                if (hitData)
                {
                    elementSelected = hitData.collider.gameObject;
                    OnElementSelected(elementSelected);
                }
            }
            else
            {
                if (elementSelected.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x <= -0.5) // Derecha
                {
                    //Debug.Log("Derecha");
                    OnElementMoved(elementSelected.transform.position);
                    elementSelected = null;
                }
                else if (elementSelected.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= 0.5)  // Izquierda
                {
                    //Debug.Log("Izquierda");
                    OnElementMoved(elementSelected.transform.position);
                    elementSelected = null;
                }
                else if (elementSelected.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= -0.5) // Arriba
                {
                    //Debug.Log("Arriba");
                    OnElementMoved(elementSelected.transform.position);
                    elementSelected = null;
                }
                else if (elementSelected.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y >= 0.5)  // Abajo
                {
                    //Debug.Log("Abajo");
                    OnElementMoved(elementSelected.transform.position);
                    elementSelected = null;
                }
            }
        }
        else
        {
            elementSelected = null;
        }
    }*/
}
