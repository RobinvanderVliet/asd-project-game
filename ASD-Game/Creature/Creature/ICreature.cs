using Creature.Creature.StateMachine;

namespace Creature.Creature
{
    public interface ICreature
    {
        /// <summary>
        /// Every Creature has specific data that will be influenced by the Creature itself or by the statemachine.
        /// </summary>
        public ICreatureStateMachine CreatureStateMachine { get; }

        /// <summary>
        /// Heals Creature for x amount of health
        /// </summary>
        /// <param name="amount">Amount of health that will be added to the Creature</param>
        public void ApplyDamage(double amount);

        /// <summary>
        /// Amount of damage a Creature deals to enemies
        /// </summary>
        /// <param name="amount">Damage the creature deals to enemies</param>
        public void HealAmount(double amount);
    }
}
