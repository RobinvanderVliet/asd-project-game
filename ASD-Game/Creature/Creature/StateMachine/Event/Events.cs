namespace Creature.Creature.StateMachine.Event
{
    public class CreatureEvent
    {
        public enum Event
        {
            IDLE,
            WANDERING,
            LOST_CREATURE,
            SPOTTED_CREATURE,
            ALMOST_DEAD,
            OUT_OF_STAMINA,
            FOUND_ITEM,
            CREATURE_OUT_OF_RANGE,
            CREATURE_IN_RANGE
        };
    }
}
