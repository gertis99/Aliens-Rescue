
using UnityEngine;

public abstract class ABooster
{
    protected Grid gridModel;
    protected LevelController levelController;

    public void Initialize(LevelController levelController)
    {
        this.levelController = levelController;
    }

    public abstract void Execute(Vector2 pos, Grid model);
}
