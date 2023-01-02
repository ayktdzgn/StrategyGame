using Core.Interfaces;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Objects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Entity : MonoBehaviour, ISelectable
    {
        [SerializeField] protected EntityInfo _entityInfo;

        protected int _width;
        protected int _height;

        protected string _name;
        protected Sprite _sprite;

        protected int _health;
        protected int _currentHealth;

        public int Width => _width;
        public int Height => _height;

        public Sprite GetSprite { get => _sprite; }
        public string GetName { get => _name; }

        protected SpriteRenderer _spriteRenderer;

        protected virtual void Awake()
        {
            _width = _entityInfo.size.x;
            _height = _entityInfo.size.y;
            _name = _entityInfo.entityName;
            _sprite = _entityInfo.sprite;
            _health = _entityInfo.health;
            _currentHealth = _entityInfo.health;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _sprite;
        }
    }
}
