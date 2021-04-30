using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;

namespace Creature
{
    public class Player : ICreature
    {
        private PlayerStateMachine _playerStateMachine;
        private PlayerData _playerData;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _playerStateMachine;
        }

        public Player(PlayerData playerData)
        {
            _playerData = playerData;
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