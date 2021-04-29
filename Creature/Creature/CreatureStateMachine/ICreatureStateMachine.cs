using Creature.Creature.StateMachine.CreatureData;
using System;

namespace Creature.Creature.CreatureStateMachine
{
    interface ICreatureStateMachine
    {
        /// <summary>
        /// Every Creature has specific data that will be influenced by the Creature itself or by the statemachine.
        /// </summary>
        public ICreatureData CreatureData { get; set; }

        /// <summary>
        /// Starts the ICreatureStateMachine using a custom RuleSet.
        /// </summary>
        /// <param name="ruleSet">RuleSet that is used to start the ICreatureStateMachine</param>
        public void StartStateMachine(RuleSet ruleSet);

        /// <summary>
        /// Fire events on a ICreatureStateMachine.
        /// An event can be 'spotting a player', 'being attacked', etc.
        /// </summary>
        /// <param name="creatureEvent">Event that occured.</param>
        /// <param name="argument">Relevant information about this event. For example: the Player that was spotted.</param>
        public void FireEvent(Enum creatureEvent, object argument);

        /// <summary>
        /// Fire events on a ICreatureStateMachine.
        /// An event can be 'spotting a player', 'being attacked', etc.
        /// </summary>
        /// <param name="creatureEvent">Event that occured.</param>
        public void FireEvent(Enum creatureEvent);

    }
}
