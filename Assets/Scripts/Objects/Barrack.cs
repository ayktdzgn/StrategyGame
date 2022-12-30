using System.Collections;
using System.Collections.Generic;
using Core.Pathfinding;
using UnityEngine;

public class Barrack : Building, IProducer
{
    [SerializeField] Transform _spawnPointLocator;
    [SerializeField] Vector2Int _spawnPointLocatorSize = new Vector2Int(1,1);
    Vector2Int _spawnPoint;

    public Unit[] _products;
    public IProduct[] Products => _products;
    public Transform SpawnPointLocator => _spawnPointLocator;

    public Vector2Int SpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }
    public Vector2Int SpawnPointLocatorSize => _spawnPointLocatorSize;


    public void SpawnPointLocatorStatus(bool status)
    {
        _spawnPointLocator.gameObject.SetActive(status);
    }

    public void SetSpawnPointLocatorPosition(Vector2Int pos)
    {
        _spawnPoint = pos;
        _spawnPointLocator.transform.position = new Vector3(pos.x,pos.y,-2);
    }

    public void Produce(IProduct product)
    {
        var unit = UnitFactory.Instance.GetNewProduct(product.GetName);
        unit.transform.position = Pathfinding.Instance.Grid.GetGridObjectPositions(transform.position);
        unit.Move(_spawnPoint);
    }
}
