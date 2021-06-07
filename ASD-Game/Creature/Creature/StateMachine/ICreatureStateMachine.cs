using ASD_Game.Creature.Creature.StateMachine.Data;
using ASD_Game.Creature.Creature.StateMachine.Event;

namespace ASD_Game.Creature.Creature.StateMachine
{
    public interface ICreatureStateMachine
    {
        /// <summary>
        /// Every Creature has specific data that will be influenced by the Creature itself or by the statemachine.
        /// </summary>
        public ICreatureData CreatureData { get; set; }

        /// <summary>
        /// Starts the ICreatureStateMachine using a custom RuleSet.
        /// </summary>
        public void StartStateMachine();

        /// <summary>
        /// Fire events on a ICreatureStateMachine.
        /// An event can be 'spotting a player', 'being attacked', etc.
        /// </summary>
        /// <param name="creatureEvent">Event that occured.</param>
        /// <param name="argument">Relevant information about this event. For example: the ActionHandling that was spotted.</param>
        public void FireEvent(CreatureEvent.Event creatureEvent, object argument);

        /// <summary>
        /// Fire events on a ICreatureStateMachine.
        /// An event can be 'spotting a player', 'being attacked', etc.
        /// </summary>
        /// <param name="creatureEvent">Event that occured.</param>
        public void FireEvent(CreatureEvent.Event creatureEvent);
    }
}
