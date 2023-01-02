using System;
public interface IAttacker
{
    public void Attack(IAttackable attackable);
    public void StopAttack();
}
