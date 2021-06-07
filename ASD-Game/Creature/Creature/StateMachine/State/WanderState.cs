using System;
using System.Collections.Generic;
using ActionHandling;
using ASD_project.Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Network;
using WorldGeneration;

namespace Creature.Creature.StateMachine.State
{
    public class WanderState : CreatureState
    {
        private MoveHandler _moveHandler =
            new MoveHandler(new ClientController(new NetworkComponent()), new WorldService());

        public WanderState(ICreatureData creatureData, ICreatureStateMachine stateMachine,
            List<BuilderInfo> builderInfoList, BuilderConfigurator builderConfiguration) : base(creatureData,
            stateMachine, builderInfoList, builderConfiguration)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
            _builderConfiguration = builderConfiguration;
            _builderInfoList = builderInfoList;
        }

        public WanderState(ICreatureData creatureData, ICreatureStateMachine stateMachine) : base(creatureData,
            stateMachine)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }

        public override void Do()
        {
            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "wander")
                {
                    if (_builderConfiguration.GetGuard(_creatureData, _target, builderInfo.RuleSets, "wander"))
                    {
                        //TODO implement Attack logic + gather targetData
                        int steps = new Random().Next(10);
                        _moveHandler.SendMove(pickRandomDirection(), steps);
                    }
                }
            }
        }

        private string pickRandomDirection()
        {
            String _direction = "";
            int CaseSwitch = new Random().Next(1, 4);
            switch (CaseSwitch)
            {
                case 1:
                    _direction += "up";
                    break;
                case 2:
                    _direction += "right";
                    break;
                case 3:
                    _direction += "down";
                    break;
                case 4:
                    _direction += "left";
                    break;
            }

            return _direction;
        }
    }
}