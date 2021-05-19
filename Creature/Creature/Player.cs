using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.CustomRuleSet;
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

        public Player(PlayerData playerData, RuleSet ruleSet)
        {
            _playerStateMachine = new(playerData, ruleSet);
            _playerStateMachine.StartStateMachine();
        }

        public void ApplyDamage(double amount)
        {
            _health -= amount;
            Console.WriteLine("You suffered: " + amount + " damage. Remaining health: " + _health + ".");
            if (_health < 0)
                _alive = false;
        }

        public void HealAmount(double amount)
        {
            _playerStateMachine.CreatureData.Health += amount;
        }
    }
}