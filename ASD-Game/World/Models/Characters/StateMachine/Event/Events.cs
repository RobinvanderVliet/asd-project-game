namespace WorldGeneration.StateMachine.Event
{
    public class CharacterEvent
    {
        public enum Event
        {
            IDLE,
            WANDERING,
            LOST_CREATURE,
            SPOTTED_CREATURE,

            // Generic Events
            DO,

            // Specific Creature Event
            LOST_PLAYER,
            SPOTTED_PLAYER,
            ALMOST_DEAD,
            OUT_OF_STAMINA,
            FOUND_ITEM,
            CREATURE_OUT_OF_RANGE,
            CREATURE_IN_RANGE
        };
    }
}