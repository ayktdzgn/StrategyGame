using UnityEngine;

namespace Core.Interfaces
{
    public interface IProduct
    {
        public Sprite GetSprite { get; }
        public string GetName { get; }
    }
}
