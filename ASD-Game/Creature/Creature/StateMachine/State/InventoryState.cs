

using System;
using System.Collections.Generic;
using ASD_project.Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.StateMachine.State
{
    public class InventoryState : CreatureState
    {
        public InventoryState(ICreatureData creatureData, ICreatureStateMachine stateMachine, List<BuilderInfo> builderInfoList, BuilderConfigurator builderConfiguration) : base(creatureData, stateMachine, builderInfoList, builderConfiguration)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
            _builderConfiguration = builderConfiguration;
            _builderInfoList = builderInfoList;
        }
        
        public InventoryState(ICreatureData creatureData, ICreatureStateMachine stateMachine) : base (creatureData, stateMachine)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }

        public override void Do()
        {
            // foreach (var builderInfo in _builderInfoList)
            // {
            // if (builderInfo.Action == "attack")
            // {
            //     if (_builderConfiguration.GetGuard(_creatureData, _target, builderInfo.RuleSets, "item"))
            //     {
            //         //TODO implement Attack logic + gather targetData
            //     }
            // }
        }
    }
}