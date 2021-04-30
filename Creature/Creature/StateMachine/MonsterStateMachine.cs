using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;

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
            if (creatureEvent.GetType() == typeof(CreatureEvent))
            {
                _passiveStateMachine.Fire(creatureEvent, argument);
            }
        }

        public void FireEvent(CreatureEvent creatureEvent)
        {
            if (creatureEvent.GetType() == typeof(CreatureEvent))
            {
                _passiveStateMachine.Fire(creatureEvent);
            }
        }

        public void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, CreatureEvent>();

            CreatureState followPlayerState = new FollowPlayerState();
            CreatureState wanderState = new WanderState();
            CreatureState useConsumableState = new UseConsumableState();
            CreatureState attackPlayerState = new AttackPlayerState();

            CreatureEvent lostPlayerEvent = new LostPlayerEvent();
            CreatureEvent spottedPlayerEvent = new SpottedPlayerEvent();
            CreatureEvent regainedHealthAndOutOfRangeEvent = new RegainedHealthAndOutOfRangeEvent();
            CreatureEvent playerOutOfRangeEvent = new PlayerOutOfRangeEvent();
            CreatureEvent playerInRangeEvent = new PlayerInRangeEvent();
            CreatureEvent AlmostDeadEvent = new AlmostDeadEvent();

            // Wandering
            builder.In(followPlayerState).On(lostPlayerEvent).Goto(wanderState);

            // Follow player
            builder.In(wanderState).On(spottedPlayerEvent).Goto(followPlayerState).Execute<ICreature>(OnFollowPlayer);
            builder.In(useConsumableState).On(regainedHealthAndOutOfRangeEvent).Goto(followPlayerState).Execute<ICreature>(OnFollowPlayer);
            builder.In(attackPlayerState).On(playerOutOfRangeEvent).Goto(followPlayerState).Execute<ICreature>(OnFollowPlayer);

            // Attack player
            builder.In(followPlayerState).On(playerInRangeEvent).Goto(attackPlayerState).Execute<ICreature>(OnAttackPlayer);
            builder.In(attackPlayerState).On(playerInRangeEvent).Execute<ICreature>(OnAttackPlayer);
            builder.In(useConsumableState).On(regainedHealthAndOutOfRangeEvent).Goto(attackPlayerState).Execute<ICreature>(OnAttackPlayer);

            // Use potion
            builder.In(attackPlayerState).On(AlmostDeadEvent).Goto(useConsumableState).Execute<IConsumable>(OnUseConsumable);
            builder.In(followPlayerState).On(AlmostDeadEvent).Goto(useConsumableState).Execute<IConsumable>(OnUseConsumable);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
    }
}
