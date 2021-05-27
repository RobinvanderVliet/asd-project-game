using System;
using System.Collections.Generic;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;
using Creature.Exception;

namespace Creature.Creature.StateMachine
{
    class MonsterStateMachine : ICreatureStateMachine
    {
        private PassiveStateMachine<CreatureState, CreatureEvent.Event> _passiveStateMachine;
        private MonsterData _monsterData;

        private Stack<Setting> _settings;

        private CreatureState _followPlayerState;
        private CreatureState _wanderState;
        private CreatureState _useConsumableState;
        private CreatureState _attackPlayerState;
        private CreatureState _fleeFromCreatureState;

        public MonsterStateMachine(MonsterData monsterData)
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

            _settings = new Stack<Setting>();
            _followPlayerState = new FollowCreatureState(CreatureData);
            _wanderState = new WanderState(CreatureData);
            _useConsumableState = new UseConsumableState(CreatureData);
            _attackPlayerState = new AttackState(CreatureData);
            _fleeFromCreatureState = new FleeFromCreatureState(CreatureData);

            // Wandering
            builder.In(_followPlayerState).On(CreatureEvent.Event.LOST_PLAYER).Goto(_wanderState);

            foreach (var setting in CreatureData.RuleSet)
            {
                if (_settings.Count > 0)
                {
                    var peek =
                        _settings.Peek().Property[.._settings.Peek().Property.IndexOf("_", StringComparison.Ordinal)];
                    var current =
                        setting.Property[..setting.Property.IndexOf("_", StringComparison.Ordinal)];

                    if (peek == current)
                    {
                        _settings.Push(setting);
                    }
                    else if (setting.Property.StartsWith("explore"))
                    {
                        AddExploreBehavior(setting, builder);
                    }
                    else if (setting.Property.StartsWith("combat"))
                    {
                        AddCombatBehavior(setting, builder);
                    }
                    else
                    {
                        throw new NotSupportedSettingException();
                    }
                }
                else
                {
                    _settings.Push(setting);
                }
            }

            builder.In(_useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_PLAYER_IN_RANGE)
                .Goto(_attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);

            // Use potion
            builder.In(_attackPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(_useConsumableState)
                .Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            builder.In(_followPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(_useConsumableState)
                .Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);

            builder.WithInitialState(_useConsumableState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }

        private void AddExploreBehavior(Setting setting,
            StateMachineDefinitionBuilder<CreatureState, CreatureEvent.Event> builder)
        {
            _settings.Clear();
        }

        private void AddCombatBehavior(Setting setting,
            StateMachineDefinitionBuilder<CreatureState, CreatureEvent.Event> builder)
        {
            _settings.Clear();
        }
    }
}