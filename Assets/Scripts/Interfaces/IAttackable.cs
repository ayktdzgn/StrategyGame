using System;
using UnityEngine;

public interface IAttackable
{
    public IAttackable GetDamage(int damage);
    public Vector2Int GetSize();
    public Vector2 GetPosition();
    public bool IsAlive { get; }
}
