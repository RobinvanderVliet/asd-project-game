namespace Creature.Creature.StateMachine.Event
{
    public class CreatureEvent
    {
        public enum Event
        {
            IDLE,
            LOST_CREATURE,
            SPOTTED_CREATURE,
            ALMOST_DEAD,
            FOUND_ITEM,
            REGAINED_HEALTH_CREATURE_OUT_OF_RANGE,
            REGAINED_HEALTH_CREATURE_IN_RANGE,
            CREATURE_OUT_OF_RANGE,
            CREATURE_IN_RANGE
        };
    }
}
