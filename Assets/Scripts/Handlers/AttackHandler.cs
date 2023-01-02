using System.Collections;
using System.Collections.Generic;
using Core.Grid;
using UnityEngine;

public static class AttackHandler
{
    public static IAttackable Attack(Transform transform, IAttackable attackable, int damage, GenericGrid<Tile> grid, int attackRange, Vector2Int attackableSize, Vector2 attackablePosition)
    {
        for (int x = 0; x < attackableSize.x; x++)
        {
            for (int y = 0; y < attackableSize.y; y++)
            {
                Vector2 newAttackablePos = new Vector2(attackablePosition.x + (grid.GetCellSize() * x) , attackablePosition.y + (grid.GetCellSize() * y));
                if (Vector2.Distance(transform.position, newAttackablePos) <= attackRange)
                {
                    return attackable.GetDamage(damage);
                }
            }
        }
        return null;
    }
}

