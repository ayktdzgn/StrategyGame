using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Info", menuName = "Scriptable Objects/Unit Info", order = 0)]
public class UnitInfo : EntityInfo
{
    public int damage;
    public int attackRange;
    public float attackCooldown;

    public int movementSpeed;
    public Color selectedColor;
}
