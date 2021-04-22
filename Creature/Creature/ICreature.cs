using Appccelerate.StateMachine.AsyncMachine;
using Creature.Creature;
using Creature.Pathfinder;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Creature
{
    public interface ICreature
    {
        bool IsAlive { get; set; }
        Vector2 Position { get; set; }
        int VisionRange { get; set; }

        /// <summary>
        /// Starts the defaultstate machine using the default RuleSet for this Creature.
        /// </summary>
        public void StartStateMachine();

        /// <summary>
        /// Starts the statemachine using a custom RuleSet.
        /// </summary>
        /// <param name="ruleSet">RuleSet that is used to start the statemachine</param>
        public void StartStateMachine(RuleSet ruleSet);

        /// <summary>
        /// Fire events on a Creature.
        /// Creatures will adapt their behavior to specific events.
        /// An event can be 'spotting a player', 'being attacked', etc.
        /// </summary>
        /// <param name="creatureEvent">Enum that specifies what Event occured. Use events defined by the relevant ICreature implementation.</param>
        /// <param name="argument">Relevant information about this event. For example: the Player that was spotted.</param>
        public void FireEvent(Enum creatureEvent, object argument);

        /// <summary>
        /// Fire events on a Creature.
        /// Creatures will adapt their behavior to specific events.
        /// An event can be 'spotting a player', 'being attacked', etc.
        /// </summary>
        /// <param name="creatureEvent">Enum that specifies what Event occured. Use events defined by the relevant ICreature implementation.</param>
        public void FireEvent(Enum creatureEvent);

        public void ApplyDamage(double amount);

        public void HealAmount(double amount);

        /// <summary>
        /// Executes behavior.
        /// </summary>
        public void Do(Stack<Node> path);
    }
}
