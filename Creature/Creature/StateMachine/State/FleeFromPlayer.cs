using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.StateMachine.State
{
    public class FleeFromPlayer : CreatureState
    {
        public FleeFromPlayer(ICreatureData creatureData) : base(creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Do(ICreatureData creatureData)
        {
            
        }
    }
}