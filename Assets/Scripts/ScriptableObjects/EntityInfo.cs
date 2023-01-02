using UnityEngine;

namespace Core.ScriptableObjects
{
    public class EntityInfo : ScriptableObject
    {
        public Sprite sprite;
        public Vector2Int size;
        public string entityName;
        public int health;
    }
}
