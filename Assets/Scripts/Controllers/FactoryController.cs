using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    TileFactory _tileFactory;
    UnitFactory _unitFactory;
    BuildingFactory _buildingFactory;

    public TileFactory TileFactory => _tileFactory;
    public UnitFactory UnitFacory => _unitFactory;
    public BuildingFactory BuildingFactory => _buildingFactory;

    private void Awake()
    {
        _tileFactory = GetComponentInChildren<TileFactory>();
        _unitFactory = GetComponentInChildren<UnitFactory>();
        _buildingFactory = GetComponentInChildren<BuildingFactory>();
    }
}
