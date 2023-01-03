using Core.Controllers;
using Core.Interfaces;
using Core.Factory;
using UnityEngine;

namespace Core.Objects
{
    public class Building : Entity, IAttackable
    {
        bool _isAlive = true;
        public bool IsAlive => _isAlive;
        // Set given damage to object. If health value goes belove to 0 then it will die
        public IAttackable GetDamage(int damage)
        {
            _currentHealth = _currentHealth - damage >= 0 ? _currentHealth - damage : 0;
            if (_currentHealth == 0)
            {
                Die();
                return null;
            }
            return this;
        }
        //Refuse object to factory and relase occupied tiles
        public virtual void Die()
        {
            _isAlive = false;
            BuildingFactory.Instance.RefuseProduct(_name, this);
            GameController.Instance.GridController.ReleaseGridTiles(GetPosition(), _width, _height);
        }

        public Vector2Int GetSize()
        {
            return new Vector2Int(_width, _height);
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }
    }
}
