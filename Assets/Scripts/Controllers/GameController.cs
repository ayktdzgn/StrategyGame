using System.Collections;
using System.Collections.Generic;
using Core.Pathfinding;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] GameView _gameView;
    GridController _gridController;
    FactoryController _factoryController;
    InputController _inputController;

    public GridController GridController => _gridController;

    private void Start()
    {
        _gridController = GetComponentInChildren<GridController>();
        _factoryController = GetComponentInChildren<FactoryController>();
        _inputController = GetComponentInChildren<InputController>();

        _gameView.Bind(this);
        Init();
    }

    void Init()
    {
        _gridController.GenerateGrid(_factoryController);
    }

    //If we needed State Pattern
    private void Update()
    {
        _inputController.InputUpdate();
    }

    public void CarryingBuild(string buildName)
    {
        if (_inputController.IsCarryingBuilding) return;
        Building building = _factoryController.BuildingFactory.GetNewProduct(buildName);
        _inputController.BuildingCarry(ref building);
    }
}
