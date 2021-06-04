using Creature.Creature.StateMachine.Data;
using System;
using System.Collections.Generic;
using ASD_project.Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Builder;

namespace Creature.Creature.StateMachine.State
{
    public class EngageState : CreatureState
    {
        public EngageState(ICreatureData creatureData, ICreatureStateMachine stateMachine, List<BuilderInfo> builderInfoList, BuilderConfiguration builderConfiguration) : base(creatureData, stateMachine, builderInfoList, builderConfiguration)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
            _builderConfiguration = builderConfiguration;
            _builderInfoList = builderInfoList;
        }
        
        public EngageState(ICreatureData creatureData, ICreatureStateMachine stateMachine) : base (creatureData, stateMachine) 
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }

        public override void Do()
        {
            //TODO implement logic
            throw new NotImplementedException();
        }
    }
}