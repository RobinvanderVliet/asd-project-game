using Creature.Creature.StateMachine.Data;
using System;
using System.Collections.Generic;
using ASD_project.Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Builder;

namespace Creature.Creature.StateMachine.State
{
    public class CollectState : CreatureState
    {
        public CollectState(ICreatureData creatureData, ICreatureStateMachine stateMachine, List<BuilderInfo> builderInfoList, BuilderConfiguration builderConfiguration) : base(creatureData, stateMachine, builderInfoList, builderConfiguration)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
            _builderConfiguration = builderConfiguration;
            _builderInfoList = builderInfoList;
        }

        public override void Do()
        {
            //TODO implement logic
            throw new NotImplementedException();
        }
    }
}
