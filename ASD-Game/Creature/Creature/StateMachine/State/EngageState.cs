using Creature.Creature.StateMachine.Data;
using System.Collections.Generic;
using Creature.Creature.StateMachine.Builder;

namespace Creature.Creature.StateMachine.State
{
    public class EngageState : CreatureState
    {
        public EngageState(ICreatureData creatureData, ICreatureStateMachine stateMachine, List<BuilderInfo> builderInfoList, BuilderConfigurator builderConfiguration) : base(creatureData, stateMachine, builderInfoList, builderConfiguration)
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
            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "engage")
                {
                    if (_builderConfiguration.GetGuard(_creatureData, _target, builderInfo))
                    {
                        //TODO implement Attack logic + gather targetData
                    }
                }
            }
        }
    }
}