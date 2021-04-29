using Creature.Creature.CreatureStateMachine;

namespace Creature
{
    public class Player : ICreature
    {
        private PlayerStateMachine _playerStateMachine;

        public void ApplyDamage(double amount)
        {
            throw new System.NotImplementedException();
        }

        public void HealAmount(double amount)
        {
            throw new System.NotImplementedException();
        }
    }
}