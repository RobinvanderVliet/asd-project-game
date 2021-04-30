using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;

namespace Creature
{
    public class Monster : ICreature
    {
        private MonsterStateMachine _monsterStateMachine;

        public Monster(MonsterData monsterData)
        {

        }

        public void ApplyDamage(double amount)
        {
            // TODO: Get from statemachine
        }

        public void HealAmount(double amount)
        {
            // TODO: get from statemachine
        }
    }
}
