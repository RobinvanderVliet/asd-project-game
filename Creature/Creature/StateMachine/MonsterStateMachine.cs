using System;
using System.Collections.Generic;
using System.Linq;
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

        private CreatureState _followPlayerState;
        private CreatureState _wanderState;
        private CreatureState _useConsumableState;
        private CreatureState _attackPlayerState;
        private CreatureState _fleeFromCreatureState;

        //private CreatureEvent.Event _currentEvent;

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
            //_currentEvent = creatureEvent;
            //StartStateMachine();
            _passiveStateMachine.Fire(creatureEvent, argument);
        }

        public void FireEvent(CreatureEvent.Event creatureEvent)
        {
            //_currentEvent = creatureEvent;
            //StartStateMachine();
            _passiveStateMachine.Fire(creatureEvent);
        }

        public void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, CreatureEvent.Event>();

            //Console.WriteLine(_currentEvent);
            
            _followPlayerState = new FollowCreatureState(CreatureData);
            _wanderState = new WanderState(CreatureData);
            _useConsumableState = new UseConsumableState(CreatureData);
            _attackPlayerState = new AttackState(CreatureData);
            _fleeFromCreatureState = new FleeFromCreatureState(CreatureData);

            // Wandering
            builder.In(_followPlayerState).On(CreatureEvent.Event.LOST_CREATURE).Goto(_wanderState);

            List<RuleSet> rulesetList = RuleSetFactory.GetRuleSetListFromDictionaryList(CreatureData.RuleSet);

            //RuleSet follow = new RuleSet();
            //RuleSet attack = new RuleSet();
            //RuleSet flee = new RuleSet();

            CreatureEvent.Event followEvent;

            // check for each state (in _comparison_true of _false) if they're default or not;
            // if default, get event
            // if not default, go to default and get event
            // TODO: build conditions (if-statements) based on configuration?
            foreach (RuleSet ruleSet in rulesetList)
            {
                if (ruleSet.ComparisonTrue == "follow" || ruleSet.ComparisonFalse == "follow")
                {
                    if (ruleSet.Action == "default")
                    {
                        followEvent = GetEvent(ruleSet.Comparable, ruleSet.Threshold, ruleSet.Comparison);
                    }
                    else
                    {
                        foreach (RuleSet ruleSet2 in rulesetList)
                        {
                            if (ruleSet2.Action == "default" && (ruleSet2.ComparisonTrue == "follow" || ruleSet2.ComparisonFalse == "follow"))
                            {
                                followEvent = GetEvent(ruleSet2.Comparable, ruleSet2.Threshold, ruleSet2.Comparison);
                            }
                        }
                    }
                    
                }
            }

            //builder.In(_followPlayerState).ExecuteOnEntry<ICreatureData>(new AttackState(CreatureData).Do);

            //builder.In(_followPlayerState).On(CreatureEvent.Event.CREATURE_IN_RANGE).
            //    If<ICreatureData>((c) => typeof(PlayerData) == c.GetType()).
            //    Goto(_attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);

            // Follow player
            builder.In(_wanderState).On(CreatureEvent.Event.SPOTTED_CREATURE).
                Goto(_followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            builder.In(_followPlayerState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(_followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            builder.In(_useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_CREATURE_OUT_OF_RANGE).Goto(_followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            builder.In(_attackPlayerState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(_followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);

            // Attack player
            builder.In(_followPlayerState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Goto(_attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);
            builder.In(_attackPlayerState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Execute<ICreatureData>(new AttackState(CreatureData).Do);
            builder.In(_useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_CREATURE_IN_RANGE).Goto(_attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);

            // Use potion
            builder.In(_attackPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(_useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            builder.In(_followPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(_useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);

            builder.WithInitialState(_wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
        public CreatureEvent.Event GetEvent(string comparable, string threshold, string comparison)
        {
            if ((comparable == "monster" || comparable == "agent") && (threshold == "monster" || threshold == "agent"))
            {
                if (comparison == "sees" || comparison == "finds")
                {
                    return CreatureEvent.Event.SPOTTED_CREATURE;
                }
                else if (comparison == "nearby")
                {
                    return CreatureEvent.Event.CREATURE_IN_RANGE;
                }
                else if (comparison == "lost")
                {
                    return CreatureEvent.Event.LOST_CREATURE;
                }
            }
            else if ((comparable == "monster" || comparable == "agent") && threshold == "item")
            {
                if (comparison == "finds")
                {
                    return CreatureEvent.Event.FOUND_ITEM;
                }
            }
            return CreatureEvent.Event.IDLE;
        }
    }
}