using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity, IAttackable
{
    bool _isAlive = true;
    public bool IsAlive => _isAlive;

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

    public virtual void Die()
    {
        _isAlive = false;
        BuildingFactory.Instance.RefuseProduct(_name, this);
        GameController.Instance.GridController.ReleaseGridTiles(GetPosition(),_width,_height);
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
