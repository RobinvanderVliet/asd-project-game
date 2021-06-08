using System.Collections.Generic;
using Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.StateMachine.State
{
    public class IdleState : CreatureState
    {
        public IdleState(ICreatureData creatureData, ICreatureStateMachine stateMachine,
            List<BuilderInfo> builderInfoList, BuilderConfigurator builderConfiguration) : base(creatureData,
            stateMachine, builderInfoList, builderConfiguration)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
            _builderConfiguration = builderConfiguration;
            _builderInfoList = builderInfoList;
        }

        public IdleState(ICreatureData creatureData, ICreatureStateMachine stateMachine) : base(creatureData,
            stateMachine)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }

        public void Do()
        {
            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "idle")
                {
                    if (_builderConfiguration.GetGuard(_creatureData, _target, builderInfo))
                    {
                        //Idle state is meant for the AI to be able to do nothing, but check if it needs to do something. in Order to save CPU utilization
                    }
                }
            }
        }
    }
}