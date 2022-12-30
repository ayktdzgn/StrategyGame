using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Building, IProductable
{
    [SerializeField] Transform _spawnPointLocator;
    Vector2Int _spawnPoint;

    public Unit[] _products;
    public IProduct[] Products => _products;
    public Transform SpawnPointLocator => _spawnPointLocator;

    public Vector2Int SpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }

    public void SpawnPointLocatorStatus(bool status)
    {
        _spawnPointLocator.gameObject.SetActive(status);
    }

    public void SetSpawnPointLocatorPosition(Vector2Int pos)
    {
        _spawnPoint = pos;
        _spawnPointLocator.transform.position = (Vector2)pos;
    }
}
