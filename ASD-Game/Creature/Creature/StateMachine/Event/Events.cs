namespace ASD_Game.Creature.Creature.StateMachine.Event
{
    public class CreatureEvent
    {
        public enum Event
        {
            LostPlayer,
            SpottedPlayer,
            AlmostDead,
            RegainedHealthPlayerOutOfRange,
            RegainedHealthPlayerInRange,
            PlayerOutOfRange,
            PlayerInRange
        };
    }
}
