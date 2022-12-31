using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Grid
{
public class Tile
{
    private GenericGrid<Tile> _grid;
    private int _x;
    private int _y;
    //G = walking cost from the start node
    public int gCost;
    //H = heuristic cost to reach end node this, estimated calculation is done without adding walls or any blocking objects
    public int hCost;
    //F = G + H
    public int fCost;

    public Tile cameFromNode;

    bool _isBuildable = true;
    bool _isWalkable;
    bool _isOccupiedByUnit;
    public bool isBuildable
    {
        get { return _isBuildable; }
        set { _isBuildable = value; }
    }

    public bool isWalkable
    {
        get { return _isWalkable; }
        set { _isWalkable = value; }
    }

    public bool isOccupiedByUnit
    {
        get { return _isOccupiedByUnit; }
        set { _isOccupiedByUnit = value; }
    }


    public Tile(GenericGrid<Tile> grid, int x, int y)
    {
        this._grid = grid;
        this._x = x;
        this._y = y;
        _isWalkable = true;
        _isOccupiedByUnit = false;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public Vector3 GetTileWorldPositions()
    {
        return _grid.GetWorldPosition(GetX(), GetY());
    }

    public Vector2 GetXY()
    {
        return new Vector2(_x, _y);
    }

    public int GetX()
    {
        return _x;
    }
    public int GetY()
    {
        return _y;
    }
}
}