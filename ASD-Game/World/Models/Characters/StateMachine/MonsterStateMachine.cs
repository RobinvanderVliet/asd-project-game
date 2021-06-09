using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.State;
using World.Models.Characters.StateMachine.Builder;
using World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine.Event;
using WorldGeneration.StateMachine.State;

namespace WorldGeneration.StateMachine
{
    public class MonsterStateMachine : DefaultStateMachine
    {
        private Timer _timer;

        public MonsterStateMachine(ICharacterData characterData) : base(characterData)
        {
        }

        [ExcludeFromCodeCoverage]
        public ICharacterData CharacterData
        {
            get => _characterData;
            set => _characterData = (MonsterData) value;
        }

        public override void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CharacterState, CharacterEvent.Event>();
            var ruleSetFactory = new RuleSetFactory();
            var rulesetList = ruleSetFactory.GetRuleSetListFromSettingsList(CharacterData.RuleSet);
            var builderConfigurator = new BuilderConfigurator(rulesetList, CharacterData, this);
            var builderInfoList = builderConfigurator.GetBuilderInfoList();

            CharacterData.BuilderConfigurator = builderConfigurator;
            
            CharacterState followCreatureState = new FollowCreatureState(CharacterData);
            CharacterState wanderState = new WanderState(CharacterData);
            CharacterState useConsumableState = new UseConsumableState(CharacterData);
            CharacterState attackState = new AttackState(CharacterData);

            DefineDefaultBehaviour(ref builder, ref followCreatureState);
            DefineDefaultBehaviour(ref builder, ref wanderState);
            DefineDefaultBehaviour(ref builder, ref useConsumableState);
            DefineDefaultBehaviour(ref builder, ref attackState);

            foreach (BuilderInfo builderInfo in builderInfoList)
            {
                foreach (var initialState in builderInfo.InitialStates)
                {
                    builder.In(initialState).On(builderInfo.Event)
                        .If<object>((targetData) => builderConfigurator.GetGuard(_characterData, targetData, builderInfo))
                        .Goto(builderInfo.TargetState).Execute<ICharacterData>(builderInfo.TargetState.SetTargetData);
                }
            }

            foreach (var action in builderConfigurator.GetActionWithStateList())
            {
                if (!builderInfoList.Any(x => x.Action == action.Key))
                {
                    if (action.Key == "wander")
                    {
                        // Wandering
                        builder.In(followCreatureState).On(CharacterEvent.Event.LOST_CREATURE).Goto(wanderState);
                    }
                    else if (action.Key == "follow")
                    {
                        // Follow player
                        builder.In(wanderState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState)
                            .Execute<ICharacterData>(followCreatureState.SetTargetData);
                        builder.In(followCreatureState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState)
                            .Execute<ICharacterData>(followCreatureState.SetTargetData);
                        builder.In(attackState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState)
                            .Execute<ICharacterData>(followCreatureState.SetTargetData);
                    }
                    else if (action.Key == "attack")
                    {
                        // Attack player
                        builder.In(followCreatureState).On(CharacterEvent.Event.CREATURE_IN_RANGE)
                            .Goto(attackState).Execute<ICharacterData>(attackState.SetTargetData);
                        builder.In(attackState).On(CharacterEvent.Event.CREATURE_IN_RANGE)
                            .Execute<ICharacterData>(attackState.SetTargetData);
                    }
                    //else if (action == "...")
                }
            }

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();

            // Start State machine loop
            Update();
        }
    }
}