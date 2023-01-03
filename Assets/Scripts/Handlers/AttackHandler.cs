using System.Collections;
using System.Collections.Generic;
using Core.Grid;
using Core.Interfaces;
using UnityEngine;

namespace Core.Handlers
{
    public static class AttackHandler
    {
        /// <summary>
        /// Setup attack to IAttackable object
        /// </summary>
        /// <param name="transform"> IAttacker transform </param>
        /// <param name="attackable"> IAttackable object which attacked by attacker </param>
        /// <param name="damage"> Damage value to give IAttackable</param>
        /// <param name="grid"> Base grid </param>
        /// <param name="attackRange"> IAttacker attack range </param>
        /// <param name="attackableSize"> IAttackable width and height </param>
        /// <param name="attackablePosition"> IAttackable object world position </param>
        /// <returns> Get IAttackable object for check if it is null </returns>
        public static IAttackable Attack(Transform transform, IAttackable attackable, int damage, GenericGrid<Tile> grid, int attackRange, Vector2Int attackableSize, Vector2 attackablePosition)
        {
            for (int x = 0; x < attackableSize.x; x++)
            {
                for (int y = 0; y < attackableSize.y; y++)
                {
                    Vector2 newAttackablePos = new Vector2(attackablePosition.x + (grid.GetCellSize() * x), attackablePosition.y + (grid.GetCellSize() * y));
                    if (Vector2.Distance(transform.position, newAttackablePos) <= attackRange)
                    {
                        return attackable.GetDamage(damage);
                    }
                }
            }
            return null;
        }
    }
}

