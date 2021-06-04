using System.Collections.Generic;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.State;
using System.Linq;
using ASD_project.Creature.Creature.StateMachine.Builder;

namespace Creature.Creature.StateMachine
{
    class MonsterStateMachine : ICreatureStateMachine
    {
        private PassiveStateMachine<CreatureState, CreatureEvent.Event> _passiveStateMachine;
        private ICreatureData _monsterData;
        
        private CreatureState _followPlayerState;
        private CreatureState _wanderState;
        private CreatureState _useConsumableState;
        private CreatureState _attackPlayerState;
        private CreatureState _fleeFromCreatureState;

        public MonsterStateMachine(ICreatureData monsterData)
        {
            _monsterData = monsterData;
        }

        public ICreatureData CreatureData
        {
            get => _monsterData;
            set => _monsterData = (MonsterData) value;
        }

        public void FireEvent(CreatureEvent.Event creatureEvent, object argument)
        {
            _passiveStateMachine.Fire(creatureEvent, argument);
        }

        public void FireEvent(CreatureEvent.Event creatureEvent)
        {
            _passiveStateMachine.Fire(creatureEvent);
        }

        public void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, CreatureEvent.Event>();
            List<RuleSet> rulesetList = RuleSetFactory.GetRuleSetListFromSettingsList(CreatureData.RuleSet);
            var builderConfiguration = new BuilderConfiguration(rulesetList, CreatureData, this);
            List<BuilderInfo> builderInfoList = builderConfiguration.GetBuilderInfoList();
            
            _followPlayerState = new FollowCreatureState(CreatureData, this);
            _wanderState = new WanderState(CreatureData, this);
            _useConsumableState = new UseConsumableState(CreatureData, this);
            _attackPlayerState = new AttackState(CreatureData, this);
            _fleeFromCreatureState = new FleeFromCreatureState(CreatureData, this);

            foreach (BuilderInfo builderInfo in builderInfoList)
            {
                foreach (var initialState in builderInfo.InitialStates)
                {
                    builder.In(initialState).On(builderInfo.Event).
                        If<object>((targetData) => builderConfiguration.GetGuard(CreatureData, targetData, builderInfo.RuleSets, builderInfo.Action)).
                        Goto(builderInfo.TargetState).Execute<ICreatureData>(builderInfo.TargetState.SetTargetData);
                }
            }

            foreach (var action in builderConfiguration.GetActionWithStateList())
            {
                if (!builderInfoList.Any(x => x.Action == action.Key))
                {
                    if (action.Key == "wander")
                    {
                        // Wandering
                        builder.In(_followPlayerState).On(CreatureEvent.Event.LOST_CREATURE).Goto(_wanderState);
                    }
                    else if (action.Key == "follow")
                    {
                        // Follow player
                        builder.In(_wanderState).On(CreatureEvent.Event.SPOTTED_CREATURE).
                            Goto(_followPlayerState).Execute<ICreatureData>(_followPlayerState.SetTargetData);
                        builder.In(_followPlayerState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(_followPlayerState).Execute<ICreatureData>(_followPlayerState.SetTargetData);
                        builder.In(_attackPlayerState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(_followPlayerState).Execute<ICreatureData>(_followPlayerState.SetTargetData);
                    }
                    else if (action.Key == "attack")
                    {
                        // Attack player
                        builder.In(_followPlayerState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Goto(_attackPlayerState).Execute<ICreatureData>(_attackPlayerState.SetTargetData);
                        builder.In(_attackPlayerState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Execute<ICreatureData>(_attackPlayerState.SetTargetData);
                    }
                    //else if (action == "...")
                }
            }

            builder.WithInitialState(_wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
    }
}