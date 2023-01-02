using UnityEngine;

namespace Core.Interfaces
{
    public interface IMobile
    {
        public void SetSelectedColor(bool status);
        public void Move(Vector2Int destination);
        public bool IsMooving { get; }
    }
}
