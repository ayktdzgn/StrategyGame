using System.Collections;
using System.Collections.Generic;
using Core.Pathfinding;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] GameView _gameView;
    GridController _gridController;
    InputController _inputController;
    BuildingController _buildingController;

    public GridController GridController => _gridController;
    public InputController InputController => _inputController;
    public BuildingController BuildingController => _buildingController;

    public override void Awake()
    {
        base.Awake();
        _gridController = GetComponentInChildren<GridController>();
        _inputController = GetComponentInChildren<InputController>();
        _buildingController = GetComponentInChildren<BuildingController>();
    }

    private void Start()
    {
        _gameView.Bind(this);
        Init();
    }

    private void Init()
    {
        _gridController.GenerateGrid();
    }

    //If we needed State Pattern
    private void Update()
    {
        _inputController.InputUpdate();
    }

}
