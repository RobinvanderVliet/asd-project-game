using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;

namespace Creature
{
    public class Player : ICreature
    {
        private PlayerStateMachine _playerStateMachine;

        public Player(PlayerData playerData)
        {
            
        }

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