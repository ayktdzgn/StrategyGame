using Core.Factory;
using Core.Path;
using UnityEngine;

namespace Core.Controllers
{
    public class GridController : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] Vector2Int _gridSize;
        [SerializeField] float _cellSize = 1f;
        [SerializeField] int _pixelCellSize = 32;

        Pathfinding _pathfindingGrid;

        // Generate Grid
        public void GenerateGrid()
        {
            Vector3 orginPos = new Vector3(0 - Mathf.RoundToInt(_gridSize.x / 2) * _cellSize, 0 - Mathf.RoundToInt(_gridSize.y / 2) * _cellSize, 0);
            _pathfindingGrid = new Pathfinding(_gridSize.x, _gridSize.y, _cellSize, orginPos);
            GenerateGridTiles();
        }
        //Spawn Tiles to grid cells
        private void GenerateGridTiles()
        {
            for (int x = 0; x < _pathfindingGrid.Grid.GetWidth(); x++)
            {
                for (int y = 0; y < _pathfindingGrid.Grid.GetHeight(); y++)
                {
                    var tile = TileFactory.Instance.GetNewProduct("Tile");
                    tile.transform.position = new Vector3(_pathfindingGrid.Grid.GetWorldPosition(x, y).x, _pathfindingGrid.Grid.GetWorldPosition(x, y).y, 1);
                    tile.transform.localScale = _cellSize * ((float)_pixelCellSize / 10f) * Vector3.one;
                }
            }
        }
        /// <summary>
        /// Check grid cells are avaible
        /// </summary>
        /// <param name="pos">cell world position</param>
        /// <param name="width">grid's width</param>
        /// <param name="height">grid's height</param>
        /// <returns>It return bool, are grid cells avaible or not</returns>
        public bool GetGridAvailability(Vector2 pos, int width, int height)
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
        /// <summary>
        /// Set grid cells as not buildible and walkable
        /// </summary>
        /// <param name="pos">Grid world position</param>
        /// <param name="width">Grid's width</param>
        /// <param name="height">Grid's height</param>
        public void SetGridBuilt(Vector2 pos, int width, int height)
        {
            _pathfindingGrid.Grid.GetGridObjectNo(pos, out int x, out int y);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _pathfindingGrid.SetTileBuildableStatus((int)(x + i), (int)(y + j), false);
                    _pathfindingGrid.SetTileWalkableStatus((int)(x + i), (int)(y + j), false);
                }
            }
        }
        /// <summary>
        /// Set grid cells as buildable and walkable
        /// </summary>
        /// <param name="pos">Grid position</param>
        /// <param name="width">Grid's width</param>
        /// <param name="height"><Grid's height/param>
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
        //Set grid cells as occupiedable
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
}
