using System.Collections;
using System.Collections.Generic;
using Core.Pathfinding;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] Vector2Int _gridSize;
    [SerializeField] float _cellSize = 1f;

    Pathfinding _pathfindingGrid;

    public void GenerateGrid(FactoryController factoryController)
    {
        Vector3 orginPos = new Vector3(0 - Mathf.RoundToInt(_gridSize.x / 2) * _cellSize, 0 - Mathf.RoundToInt(_gridSize.y / 2) * _cellSize, 0);
        _pathfindingGrid = new Pathfinding(_gridSize.x, _gridSize.y, _cellSize, orginPos);
        GenerateGridTiles(factoryController);
    }

    void GenerateGridTiles(FactoryController factoryController)
    {
        for (int x = 0; x < _pathfindingGrid.Grid.GetWidth(); x++)
        {
            for (int y = 0; y < _pathfindingGrid.Grid.GetHeight(); y++)
            {
                var tile = factoryController.TileFactory.GetNewProduct("Tile");
                tile.transform.position = _pathfindingGrid.Grid.GetWorldPosition(x, y);
            }
        }
    }

    public bool GetGridAvailability(Vector2 pos,int width, int height)
    {
        //Vector2 mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _pathfindingGrid.Grid.GetGridObjectNo(pos, out int x, out int y);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!_pathfindingGrid.GetClickedTileBuildAvailability((int)(x + i), (int)(y + j)))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void SetGridBuilt(Vector2 pos,int width, int height)
    {
        //Vector2 mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _pathfindingGrid.Grid.GetGridObjectNo(pos, out int x, out int y);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _pathfindingGrid.SetTileNotBuildable((int)(x + i), (int)(y + j));
                _pathfindingGrid.SetTileNotWalkable((int)(x + i), (int)(y + j));
            }
        }
    }
}
