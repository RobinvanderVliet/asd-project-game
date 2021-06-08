using System.Collections.Generic;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.State;
using System.Linq;

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
            RuleSetFactory ruleSetFactory = new RuleSetFactory();
            List<RuleSet> rulesetList = ruleSetFactory.GetRuleSetListFromSettingsList(CreatureData.RuleSet);
            var builderConfigurator = new BuilderConfigurator(rulesetList, CreatureData, this);
            List<BuilderInfo> builderInfoList = builderConfigurator.GetBuilderInfoList();
            
            _followPlayerState = new FollowCreatureState(CreatureData, this,builderInfoList, builderConfigurator);
            _wanderState = new WanderState(CreatureData, this, builderInfoList, builderConfigurator);
            _useConsumableState = new UseConsumableState(CreatureData, this, builderInfoList, builderConfigurator);
            _attackPlayerState = new AttackState(CreatureData, this, builderInfoList, builderConfigurator);
            _fleeFromCreatureState = new FleeFromCreatureState(CreatureData, this, builderInfoList, builderConfigurator);

            foreach (BuilderInfo builderInfo in builderInfoList)
            {
                foreach (var initialState in builderInfo.InitialStates)
                {
                    builder.In(initialState).On(builderInfo.Event).
                        If<object>((targetData) => builderConfigurator.GetGuard(CreatureData, targetData, builderInfo)).
                        Goto(builderInfo.TargetState).Execute<ICreatureData>(builderInfo.TargetState.SetTargetData);
                }
            }

            foreach (var action in builderConfigurator.GetActionWithStateList())
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

        public void StopStateMachine()
        {
            _passiveStateMachine.Stop();
        }
    }
}