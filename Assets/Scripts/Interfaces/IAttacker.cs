namespace Core.Interfaces
{
    public interface IAttacker
    {
        public void Attack(IAttackable attackable);
        public void StopAttack();
    }
}
