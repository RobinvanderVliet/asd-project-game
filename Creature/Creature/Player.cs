using Creature.Creature.StateMachine;

namespace Creature
{
    public class Player : ICreature
    {
     
        private ICreatureStateMachine _playerStateMachine;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _playerStateMachine;
        }
        
        public Player(ICreatureStateMachine playerStateMachine)
        {
            _playerStateMachine = playerStateMachine;
        }

        public void ApplyDamage(double amount)
        {
            _playerStateMachine.CreatureData.Health -= amount;
        }

        public void HealAmount(double amount)
        {
            _playerStateMachine.CreatureData.Health += amount;
        }
        public void Disconnect()
        {
            _playerStateMachine.StartStateMachine();
        }
    }
}
