using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class controll the swap mechanic
 **/

public class SwapController
{
    public delegate void SwapDone(Element[,] grid);
    public delegate void CheckedMatch(Element element);
    public delegate void MoveDone();
    public static event SwapDone OnSwapDone = Grid => { };
    public static event CheckedMatch OnCheckedMatch;
    public static event MoveDone OnMoveDone;

    private Element elementSelected;
    private bool gridCreated = false, isPossibleSwap = true;

    private Grid gridModel;
    private Element[,] gridLevel;

    public SwapController()
    {
        gridLevel = gridModel.GetGridLevel();
    }
    /*
    private void Start()
    {
        LevelController.OnGridCreated += SetGrid;
        LevelController.OnGridChanged += SetGrid;

        InputController.OnElementSelected += SetElementSelected;
        InputController.OnElementMoved += CheckTryToMove;

        WinController.OnWinChecked += IsPossibleToSwap;
        LoseController.OnLoseChecked += IsPossibleToSwap;
    }

    private void OnDisable()
    {
        LevelController.OnGridCreated -= SetGrid;
        LevelController.OnGridChanged -= SetGrid;

        InputController.OnElementSelected -= SetElementSelected;
        InputController.OnElementMoved -= CheckTryToMove;

        WinController.OnWinChecked -= IsPossibleToSwap;
        LoseController.OnLoseChecked -= IsPossibleToSwap;
    }

    
    */
    

    
}
