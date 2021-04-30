using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;
using System;

namespace Creature.Creature.StateMachine
{
    class MonsterStateMachine : ICreatureStateMachine
    {
        private RuleSet _ruleset;
        private PassiveStateMachine<CreatureState, CreatureEvent> _passiveStateMachine;
        private MonsterData _monsterData;

        public MonsterStateMachine(MonsterData monsterData, RuleSet ruleSet)
        {
            _monsterData = monsterData;
            _ruleset = ruleSet;
        }

        public ICreatureData CreatureData
        {
            get => _monsterData;
            set => _monsterData = (MonsterData)value;
        }

        public void FireEvent(CreatureEvent creatureEvent, object argument)
        {
             _passiveStateMachine.Fire(creatureEvent, argument);
        }

        public void FireEvent(CreatureEvent creatureEvent)
        {
            _passiveStateMachine.Fire(creatureEvent);
        }

        public void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, CreatureEvent>();

            CreatureState followPlayerState = new FollowPlayerState(CreatureData);
            CreatureState wanderState = new WanderState(CreatureData);
            CreatureState useConsumableState = new UseConsumableState(CreatureData);
            CreatureState attackPlayerState = new AttackPlayerState(CreatureData);

            CreatureEvent lostPlayerEvent = new LostPlayerEvent();
            CreatureEvent spottedPlayerEvent = new SpottedPlayerEvent();
            CreatureEvent regainedHealthAndOutOfRangeEvent = new RegainedHealthAndOutOfRangeEvent();
            CreatureEvent regainedHealthAndInRangeEvent = new RegainedHealthAndInRangeEvent();
            CreatureEvent playerOutOfRangeEvent = new PlayerOutOfRangeEvent();
            CreatureEvent playerInRangeEvent = new PlayerInRangeEvent();
            CreatureEvent AlmostDeadEvent = new AlmostDeadEvent();


            // Wandering
            builder.In(followPlayerState).On(lostPlayerEvent).Goto(wanderState);

            // Follow player
            builder.In(wanderState).On(spottedPlayerEvent).Goto(followPlayerState).Execute(()=> Console.WriteLine("sdjdkjsdkj"));
            builder.In(useConsumableState).On(regainedHealthAndOutOfRangeEvent).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);
            builder.In(attackPlayerState).On(playerOutOfRangeEvent).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);

            // Attack player
            builder.In(followPlayerState).On(playerInRangeEvent).Goto(attackPlayerState).Execute<ICreatureData>(new AttackPlayerState(CreatureData).Do);
            builder.In(attackPlayerState).On(playerInRangeEvent).Execute<ICreatureData>(new AttackPlayerState(CreatureData).Do);
            builder.In(useConsumableState).On(regainedHealthAndInRangeEvent).Goto(attackPlayerState).Execute<ICreatureData>(new AttackPlayerState(CreatureData).Do);

            // Use potion
            builder.In(attackPlayerState).On(AlmostDeadEvent).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            builder.In(followPlayerState).On(AlmostDeadEvent).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();

            //_passiveStateMachine.Fire(new SpottedPlayerEvent());
        }
    }
}
