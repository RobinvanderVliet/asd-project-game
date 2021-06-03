using System;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;

namespace Creature.Creature.StateMachine.State
{
    public class IdleState : CreatureState
    {
        public IdleState(ICreatureData creatureData, ICreatureStateMachine stateMachine) : base(creatureData, stateMachine)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }

        public void Do()
        {
            if (true)//Map.getmapAroundplayer returns an other player.
            {
                _stateMachine.FireEvent(CreatureEvent.Event.WANDERING);
            }
        }
    }
}