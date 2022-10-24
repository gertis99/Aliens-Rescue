using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    // Views
    [SerializeField] private LevelView levelView;
    [SerializeField] private GridView gridView;
    [SerializeField] private PointsView pointsView;

    // Controllers
    private LevelController levelController;
    private PointsController pointsController;

    // Services
    private GameConfigService gameConfig;

    private void Awake()
    {
        gameConfig = ServiceLocator.GetService<GameConfigService>();

        // Create controllers
        levelController = new LevelController(gameConfig.BoardWidth, gameConfig.BoardHeight, gameConfig.BoardColors);
        pointsController = new PointsController(levelController);
    }

    private void Start()
    {
        // Initialize views
        gridView.Initialize(levelController);
        pointsView.Initialize(pointsController);
    }
}
