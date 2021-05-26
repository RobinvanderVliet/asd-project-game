using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;

namespace Creature
{
    public class Monster : ICreature
    {
        private MonsterStateMachine _monsterStateMachine;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _monsterStateMachine;
        }

        public Monster(MonsterData monsterData)
        {
            _monsterStateMachine = new(monsterData);
            _monsterStateMachine.StartStateMachine();
        }

        public void ApplyDamage(double amount)
        {
            _monsterStateMachine.CreatureData.Health -= amount;
        }

        public void HealAmount(double amount)
        {
            _monsterStateMachine.CreatureData.Health += amount;
        }
    }
}
