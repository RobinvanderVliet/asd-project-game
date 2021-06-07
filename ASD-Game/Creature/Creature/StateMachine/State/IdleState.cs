using System.Collections.Generic;
using ASD_project.Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;

namespace Creature.Creature.StateMachine.State
{
    public class IdleState : CreatureState
    {
        public IdleState(ICreatureData creatureData, ICreatureStateMachine stateMachine, List<BuilderInfo> builderInfoList, BuilderConfigurator builderConfiguration) : base(creatureData, stateMachine, builderInfoList, builderConfiguration)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
            _builderConfiguration = builderConfiguration;
            _builderInfoList = builderInfoList;
        }

        public IdleState(ICreatureData creatureData, ICreatureStateMachine stateMachine) : base (creatureData, stateMachine)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }
        
        public void Do()
        {
        //     foreach (var builderInfo in _builderInfoList)
        //     {
        //     if (builderInfo.Action == "attack")
        //     {
        //         if (_builderConfiguration.GetGuard(_creatureData, _target, builderInfo.RuleSets, "idle"))
        //         {
        //             //TODO implement Attack logic + gather targetData
        //         }
        //     }
         }
    }
}