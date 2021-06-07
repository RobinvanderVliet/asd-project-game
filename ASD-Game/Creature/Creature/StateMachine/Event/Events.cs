
namespace Creature.Creature.StateMachine.Event
{
    public class CreatureEvent
    {
        public enum Event
        {
            LOST_PLAYER,
            SPOTTED_PLAYER,
            ALMOST_DEAD,
            REGAINED_HEALTH_PLAYER_OUT_OF_RANGE,
            REGAINED_HEALTH_PLAYER_IN_RANGE,
            PLAYER_OUT_OF_RANGE,
            PLAYER_IN_RANGE
        };
    }
}
