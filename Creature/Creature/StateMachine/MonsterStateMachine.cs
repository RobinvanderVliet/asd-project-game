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

            CreatureState followPlayer = new FollowPlayerState();
            CreatureState wanderState = new WanderState();
            CreatureState useConsumable = new UseConsumableState();
            CreatureState attackPlayerState = new AttackPlayerState();

            CreatureEvent lostPlayerEvent = new LostPlayerEvent();
            CreatureEvent spottedPlayerEvent = new SpottedPlayerEvent();


            // Wandering
            builder.In(followPlayer).On(lostPlayerEvent).Goto(wanderState);

            // Follow player
            builder.In(wanderState).On(spottedPlayerEvent).Goto(followPlayer).Execute<ICreature>(OnFollowPlayer);
            builder.In(useConsumable).On(Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(followPlayer).Execute<ICreature>(OnFollowPlayer);
            builder.In(attackPlayerState).On(Event.PLAYER_OUT_OF_RANGE).Goto(followPlayer).Execute<ICreature>(OnFollowPlayer);

            // Attack player
            builder.In(followPlayer).On(Event.PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICreature>(OnAttackPlayer);
            builder.In(attackPlayerState).On(Event.PLAYER_IN_RANGE).Execute<ICreature>(OnAttackPlayer);
            builder.In(useConsumable).On(Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICreature>(OnAttackPlayer);

            // Use potion
            builder.In(attackPlayerState).On(Event.ALMOST_DEAD).Goto(useConsumable).Execute<IConsumable>(OnUseConsumable);
            builder.In(followPlayer).On(Event.ALMOST_DEAD).Goto(useConsumable).Execute<IConsumable>(OnUseConsumable);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
    }
}
