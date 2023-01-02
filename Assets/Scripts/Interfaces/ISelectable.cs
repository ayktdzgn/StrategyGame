using UnityEngine;

namespace Core.Interfaces
{
    public interface ISelectable
    {
        public Sprite GetSprite { get; }
        public string GetName { get; }
    }
}
