using System.Collections;
using System.Collections.Generic;
using Core.Path;
using Core.Factory;
using UnityEngine;
using Core.Interfaces;

namespace Core.Objects
{
    public class Barrack : Building, IProducer
    {
        [SerializeField] Transform _spawnPointLocator;
        [SerializeField] Vector2Int _spawnPointLocatorSize = new Vector2Int(1, 1);
        Vector2Int _spawnPoint;

        public Unit[] _products;
        public IProduct[] Products => _products;
        public Transform SpawnPointLocator => _spawnPointLocator;

        public Vector2Int SpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }
        public Vector2Int SpawnPointLocatorSize => _spawnPointLocatorSize;

        //Set Locator's gameobject active
        public void SpawnPointLocatorStatus(bool status)
        {
            _spawnPointLocator.gameObject.SetActive(status);
        }
        //Set Locator position and set Spawn Point
        public void SetSpawnPointLocatorPosition(Vector2Int pos)
        {
            _spawnPoint = pos;
            _spawnPointLocator.transform.position = (Vector2)pos;
        }
        //Produce product from factory
        public void Produce(IProduct product)
        {
            var unit = UnitFactory.Instance.GetNewProduct(product.GetName);
            unit.transform.position = Pathfinding.Instance.Grid.GetGridObjectPositions(transform.position);
            unit.Move(_spawnPoint);
        }
    }
}
