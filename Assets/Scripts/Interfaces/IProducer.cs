using UnityEngine;

namespace Core.Interfaces
{
    public interface IProducer
    {
        public IProduct[] Products { get; }
        public Transform SpawnPointLocator { get; }
        public Vector2Int SpawnPointLocatorSize { get; }
        public void SpawnPointLocatorStatus(bool status);
        public void SetSpawnPointLocatorPosition(Vector2Int pos);
        public void Produce(IProduct product);
    }
}
