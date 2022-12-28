using System.Collections;
using System.Collections.Generic;
using Core.Pathfinding;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] GameView _gameView;
    GridController _gridController;
    FactoryController _factoryController;

    private void Start()
    {
        _gridController = GetComponentInChildren<GridController>();
        _factoryController = GetComponentInChildren<FactoryController>();

        _gameView.Bind(this);
        Init();
    }

    void Init()
    {
        _gridController.GenerateGrid(_factoryController);
    }
}
