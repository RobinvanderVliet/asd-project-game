using Appccelerate.StateMachine.AsyncMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature
{
    public class Monster : ICreature
    {
        public enum State { WANDERING, FOLLOW_PLAYER, ATTACK_PLAYER, USE_POTION };
        public enum Event { LOST_PLAYER, SPOTTED_PLAYER, ALMOST_DEAD, REGAINED_HEALTH_PLAYER_OUT_OF_RANGE, REGAINED_HEALTH_PLAYER_IN_RANGE, PLAYER_OUT_OF_RANGE, PLAYER_IN_RANGE };

        private StateMachineDefinition<State, Event> definition;

        public Monster()
        {
            var builder = new StateMachineDefinitionBuilder<State, Event>();
            
            // Wandering
            builder.In(State.FOLLOW_PLAYER).On(Event.LOST_PLAYER).Goto(State.WANDERING);

            // Follow Player
            builder.In(State.WANDERING).On(Event.SPOTTED_PLAYER).Goto(State.FOLLOW_PLAYER);
            builder.In(State.USE_POTION).On(Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(State.FOLLOW_PLAYER);
            builder.In(State.ATTACK_PLAYER).On(Event.PLAYER_OUT_OF_RANGE).Goto(State.FOLLOW_PLAYER);

            // Use potion
            builder.In(State.FOLLOW_PLAYER).On(Event.ALMOST_DEAD).Goto(State.USE_POTION);
            builder.In(State.FOLLOW_PLAYER).On(Event.ALMOST_DEAD).Goto(State.USE_POTION);

            // Attack player
            builder.In(State.FOLLOW_PLAYER).On(Event.PLAYER_IN_RANGE).Goto(State.ATTACK_PLAYER);
            builder.In(State.USE_POTION).On(Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(State.ATTACK_PLAYER);

            builder.WithInitialState(State.WANDERING);

            definition = builder.Build();
            var machine = definition.CreatePassiveStateMachine();
            
            // How to use the state machine
            /*machine.Start();
            
            machine.Fire(Event.SPOTTED_PLAYER);
            machine.Fire(Event.LOST_PLAYER);
            
            machine.Fire(Event.SPOTTED_PLAYER);
            machine.Fire(Event.LOST_PLAYER);*/
        }
    }
}
