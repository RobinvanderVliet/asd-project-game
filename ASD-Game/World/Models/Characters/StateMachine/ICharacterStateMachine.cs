using ASD_Game.World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine.Data;
using WorldGeneration.StateMachine.Event;

namespace WorldGeneration.StateMachine
{
    public interface ICharacterStateMachine
    {
        /// <summary>
        /// Every Creature has specific data that will be influenced by the Creature itself or by the statemachine.
        /// </summary>
        public ICharacterData CharacterData { get; set; }

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
        public void FireEvent(CharacterEvent.Event creatureEvent, object argument);

        /// <summary>
        /// Fire events on a ICreatureStateMachine.
        /// An event can be 'spotting a player', 'being attacked', etc.
        /// </summary>
        /// <param name="creatureEvent">Event that occured.</param>
        public void FireEvent(CharacterEvent.Event creatureEvent);
    }
}