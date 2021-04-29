using Creature.Creature.CreatureStateMachine;

namespace Creature
{
    public class Monster : ICreature
    {
        private MonsterStateMachine _monsterStateMachine;

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
