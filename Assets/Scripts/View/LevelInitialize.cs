using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitialize : MonoBehaviour
{
    // Views
    [SerializeField] private LevelView levelView;
    [SerializeField] private GridView gridView;
    [SerializeField] private PointsView pointsView;

    // Controllers
    private LevelController levelController;
    private PointsController pointsController;

    private void Awake()
    {
        levelController = new LevelController(8, 8, 6);
        pointsController = new PointsController(levelController);
    }

    private void Start()
    {
        gridView.Initialize(levelController);
        pointsView.Initialize(pointsController);
    }
}
