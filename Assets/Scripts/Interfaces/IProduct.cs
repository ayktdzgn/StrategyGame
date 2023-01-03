using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IProduct
    {
        public EntityInfo EntityInfo { get; }
        public Sprite GetSprite { get; }
        public string GetName { get; }
    }
}
