using System.Collections;
using System.Collections.Generic;
using Core.Pathfinding;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] Vector2Int _gridSize;
    [SerializeField] float _cellSize = 1f;
    [SerializeField] int _pixelCellSize = 32;

    Pathfinding _pathfindingGrid;

    public void GenerateGrid()
    {
        Vector3 orginPos = new Vector3(0 - Mathf.RoundToInt(_gridSize.x / 2) * _cellSize, 0 - Mathf.RoundToInt(_gridSize.y / 2) * _cellSize, 0);
        _pathfindingGrid = new Pathfinding(_gridSize.x, _gridSize.y, _cellSize, orginPos);
        GenerateGridTiles();
    }

    private void GenerateGridTiles()
    {
        for (int x = 0; x < _pathfindingGrid.Grid.GetWidth(); x++)
        {
            for (int y = 0; y < _pathfindingGrid.Grid.GetHeight(); y++)
            {
                var tile = TileFactory.Instance.GetNewProduct("Tile");
                tile.transform.position = new Vector3(_pathfindingGrid.Grid.GetWorldPosition(x, y).x, _pathfindingGrid.Grid.GetWorldPosition(x, y).y,1);
                tile.transform.localScale = _cellSize * ((float)_pixelCellSize / 10f) * Vector3.one;
            }
        }
    }

    public bool GetGridAvailability(Vector2 pos,int width, int height)
    {
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
        _pathfindingGrid.Grid.GetGridObjectNo(pos, out int x, out int y);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _pathfindingGrid.SetTileBuildableStatus((int)(x + i), (int)(y + j) , false);
                _pathfindingGrid.SetTileWalkableStatus((int)(x + i), (int)(y + j) , false);
            }
        }
    }

    public void ReleaseGridTiles(Vector2 pos, int width, int height)
    {
        _pathfindingGrid.Grid.GetGridObjectNo(pos, out int x, out int y);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _pathfindingGrid.SetTileBuildableStatus((int)(x + i), (int)(y + j), true);
                _pathfindingGrid.SetTileWalkableStatus((int)(x + i), (int)(y + j), true);
            }
        }
    }

    public void RelaseOccupition(Vector2 pos, int width, int height)
    {
        _pathfindingGrid.Grid.GetGridObjectNo(pos, out int x, out int y);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _pathfindingGrid.SetTileOccupiedStatus((int)(x + i), (int)(y + j), true);
            }
        }
    }
}
