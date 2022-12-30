using System.Collections;
using System.Collections.Generic;
using Core.Pathfinding;
using UnityEngine;

public class Barrack : Building, IProducer
{
    [SerializeField] Transform _spawnPointLocator;
    Vector2Int _spawnPoint;

    public Unit[] _products;
    public IProduct[] Products => _products;
    public Transform SpawnPointLocator => _spawnPointLocator;

    public Vector2Int SpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }

    //private void Start()
    //{
    //    Pathfinding.Instance.Grid.GetGridObjectNo(transform.position , out int x, out int y);
    //    var tile = Pathfinding.Instance.Grid.GetGridObject(x, y-1);
    //    if (tile != null)
    //    {
    //        SetSpawnPointLocatorPosition(new Vector2Int(tile.GetX(),tile.GetY()));
    //    }
    //}

    public void SpawnPointLocatorStatus(bool status)
    {
        _spawnPointLocator.gameObject.SetActive(status);
    }

    public void SetSpawnPointLocatorPosition(Vector2Int pos)
    {
        _spawnPoint = pos;
        _spawnPointLocator.transform.position = (Vector2)pos;
        Debug.Log(pos);
    }

    public void Produce(IProduct product)
    {
        var unit = UnitFactory.Instance.GetNewProduct(product.GetName);
        Debug.Log("Created Unt: " + unit.transform.position);
        unit.Move(_spawnPoint);
    }
}
