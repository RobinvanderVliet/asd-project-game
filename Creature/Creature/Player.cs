using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;

namespace Creature
{
    public class Player : ICreature
    {
        private PlayerStateMachine _playerStateMachine;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _playerStateMachine;
        }

        public Player(PlayerData playerData)
        {
            _playerStateMachine = new(playerData, null);
            _playerStateMachine.StartStateMachine();
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