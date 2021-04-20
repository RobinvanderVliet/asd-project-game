using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Consumable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature
{
    public class Monster : ICreature
    {
        /// <summary>
        /// Player that is being interacted with.
        /// Monsters can follow, attack, spot a player, etc.
        /// </summary>
        private IPlayer player;

        /// <summary>
        /// Health of a monster, when its 0 the monster is dead.
        /// </summary>
        private int health;

        /// <summary>
        /// All events that this creature is capable of responding to.
        /// Every creature can respond to its own events.
        /// </summary>
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

        /// <summary>
        /// All states that this creature is capable of activating.
        /// Every creature has its own states that it can be in.
        /// </summary>
        public enum State
        {
            WANDERING,
            FOLLOW_PLAYER,
            ATTACK_PLAYER,
            USE_CONSUMABLE
        };

        /// <summary>
        /// Passive NOT ASYNC statemachine.
        /// A statemachine will decide how a creature responds to specific events.
        /// Statemachines will decide how a creature behaves in certain events.
        /// </summary>
        private PassiveStateMachine<State, Event> stateMachine;

        public Monster()
        {
            StartStateMachine();
        }

        public void FireEvent(Enum creatureEvent, object argument)
        {
            if (creatureEvent.GetType() == typeof(Event))
            {
                stateMachine.Fire((Event)creatureEvent, argument);
            }
        }

        public void FireEvent(Enum creatureEvent)
        {
            if (creatureEvent.GetType() == typeof(Event))
            {
                stateMachine.Fire((Event)creatureEvent);
            }
        }

        private void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<State, Event>();

            // Wandering
            builder.In(State.FOLLOW_PLAYER).On(Event.LOST_PLAYER).Goto(State.WANDERING);

            // Follow player
            builder.In(State.WANDERING).On(Event.SPOTTED_PLAYER).Goto(State.FOLLOW_PLAYER).Execute<IPlayer>(OnFollowPlayer);
            builder.In(State.USE_CONSUMABLE).On(Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(State.FOLLOW_PLAYER).Execute<IPlayer>(OnFollowPlayer);
            builder.In(State.ATTACK_PLAYER).On(Event.PLAYER_OUT_OF_RANGE).Goto(State.FOLLOW_PLAYER).Execute<IPlayer>(OnFollowPlayer);

            // Attack player
            builder.In(State.FOLLOW_PLAYER).On(Event.PLAYER_IN_RANGE).Goto(State.ATTACK_PLAYER);
            builder.In(State.USE_CONSUMABLE).On(Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(State.ATTACK_PLAYER);

            // Use potion
            builder.In(State.ATTACK_PLAYER).On(Event.ALMOST_DEAD).Goto(State.USE_CONSUMABLE).Execute<IConsumable>(OnUseConsumable);
            builder.In(State.FOLLOW_PLAYER).On(Event.ALMOST_DEAD).Goto(State.USE_CONSUMABLE).Execute<IConsumable>(OnUseConsumable);

            builder.WithInitialState(State.WANDERING);

            stateMachine = builder.Build().CreatePassiveStateMachine();
            stateMachine.Start();
        }

        private void OnFollowPlayer(IPlayer player)
        {
            this.player = player;
        }

        private void OnUseConsumable(IConsumable consumable)
        {
            // Assuming this is for healing...
            health += consumable.Amount;
        }
    }
}
