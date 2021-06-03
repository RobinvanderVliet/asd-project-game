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
            if (true)//Map.getmapAroundplayer returns an other player or object or anything that should stop the machine from ideleing and start doing stuff.
            {
                _stateMachine.FireEvent(CreatureEvent.Event.WANDERING);
            }
        }
    }
}