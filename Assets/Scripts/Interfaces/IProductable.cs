using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProductable
{
    public IProduct[] Products { get; }
    public Transform SpawnPointLocator { get; }
    public void SpawnPointLocatorStatus(bool status);
    public void SetSpawnPointLocatorPosition(Vector2Int pos);
}
