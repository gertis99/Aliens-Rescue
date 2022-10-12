using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController
{

    private ABooster currentBooster;
    private Grid gridModel;
    private LevelController levelController;
    //private Action<int, int, bool> onCellDestroyed;
    //private Action<Element> onCellCreated;

    public BoosterController(Grid model, LevelController levelController)
    {
        gridModel = model;
        this.levelController = levelController;

        ActiveBoosterController.OnBoosterActived += FireBooster;
    }

    public void TryCreateBooster(int nHorizontal, int nVertical, int row, int col)
    {
        if (TryCreateColorBombBooster(nHorizontal, nVertical, row, col)) return;
        if (TryCreateBombBooster(nHorizontal, nVertical, row, col)) return;
        if (TryCreateHorizontalBooster(nHorizontal, nVertical, row, col)) return;
        if (TryCreateVerticalBooster(nHorizontal, nVertical, row, col)) return;
    }

    private bool TryCreateVerticalBooster(int nHorizontal, int nVertical, int row, int col)
    {
        if (nVertical < 4) return false;
        gridModel.GridLevel[row, col] = new Booster(row, col, BoosterType.VerticalLineBooster);
        levelController.CreateCell(gridModel.GridLevel[row, col]);
        return true;
    }

    private bool TryCreateHorizontalBooster(int nHorizontal, int nVertical, int row, int col)
    {
        if (nHorizontal < 4) return false;
        gridModel.GridLevel[row, col] = new Booster(row, col, BoosterType.HorizontalLineBooster);
        levelController.CreateCell(gridModel.GridLevel[row, col]);
        return true;
    }

    private bool TryCreateBombBooster(int nHorizontal, int nVertical, int row, int col)
    {
        if (nHorizontal < 3 || nVertical < 3) return false;
        gridModel.GridLevel[row, col] = new Booster(row, col, BoosterType.BombBooster);
        levelController.CreateCell(gridModel.GridLevel[row, col]);
        return true;
    }

    private bool TryCreateColorBombBooster(int nHorizontal, int nVertical, int row, int col)
    {
        if (nHorizontal < 5 && nVertical < 5) return false;
        gridModel.GridLevel[row, col] = new Booster(row, col, BoosterType.ColorBombBooster);
        levelController.CreateCell(gridModel.GridLevel[row, col]);
        return true;
    }

    public void FireHorizontalLineBooster(Vector2 pos, bool moveDown)
    {
        currentBooster = new HorizontalLineBooster();
        levelController.DestroyCell(gridModel.GridLevel[(int)pos.x, (int)pos.y]);
        currentBooster.Initialize(levelController);
        gridModel.GridLevel[(int)pos.x, (int)pos.y] = null;
        currentBooster.Execute(pos, gridModel);
        currentBooster = null;
        if (moveDown)
            levelController.MoveDownPieces();
    }

    public void FireVerticalLineBooster(Vector2 pos, bool moveDown)
    {
        currentBooster = new VerticalLineBooster();
        levelController.DestroyCell(gridModel.GridLevel[(int)pos.x, (int)pos.y]);
        currentBooster.Initialize(levelController);
        gridModel.GridLevel[(int)pos.x, (int)pos.y] = null;
        currentBooster.Execute(pos, gridModel);
        currentBooster = null;
        if(moveDown)
            levelController.MoveDownPieces();
    }

    public void FireBombBooster(Vector2 pos, bool moveDown)
    {
        currentBooster = new BombBooster();
        levelController.DestroyCell(gridModel.GridLevel[(int)pos.x, (int)pos.y]);
        currentBooster.Initialize(levelController);
        gridModel.GridLevel[(int)pos.x, (int)pos.y] = null;
        currentBooster.Execute(pos, gridModel);
        currentBooster = null;
        if (moveDown)
            levelController.MoveDownPieces();
    }

    public void FireColorBombBooster(Element booster, Vector2 pos, bool moveDown)
    {
        currentBooster = new ColorBombBooster();
        levelController.DestroyCell(gridModel.GridLevel[(int)pos.x, (int)pos.y]);
        currentBooster.Initialize(levelController);
        gridModel.GridLevel[booster.GetPosX(), booster.GetPosY()] = null;
        currentBooster.Execute(pos, gridModel);
        currentBooster = null;
        if (moveDown)
            levelController.MoveDownPieces();
    }

    public void FireBooster(ABooster booster, Vector2 pos)
    {
        booster.Initialize(levelController);
        booster.Execute(pos, gridModel);
        levelController.MoveDownPieces();
    }
}
