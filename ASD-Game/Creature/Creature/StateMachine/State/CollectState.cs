using Creature.Creature.StateMachine.Data;
using System;
using ASD_project.Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Builder;

namespace Creature.Creature.StateMachine.State
{
    public class CollectState : CreatureState
    {
        public CollectState(ICreatureData creatureData, ICreatureStateMachine stateMachine, BuilderInfo builderInfo, BuilderConfiguration builderConfiguration) : base(creatureData, stateMachine, builderInfo, builderConfiguration)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
            _builderConfiguration = builderConfiguration;
            _builderInfo = builderInfo;
        }

        public override void Do()
        {
            //TODO implement logic
            throw new NotImplementedException();
        }
    }
}
