using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using ASD_Game.Creature.Creature.StateMachine.Data;
using ASD_Game.Creature.Creature.StateMachine.Event;
using ASD_Game.Creature.Creature.StateMachine.State;

namespace ASD_Game.Creature.Creature.StateMachine
{
    class MonsterStateMachine : ICreatureStateMachine
    {
        private PassiveStateMachine<CreatureState, CreatureEvent.Event> _passiveStateMachine;
        private MonsterData _monsterData;

        public MonsterStateMachine(MonsterData monsterData)
        {
            _monsterData = monsterData;
        }

        public ICreatureData CreatureData
        {
            get => _monsterData;
            set => _monsterData = (MonsterData)value;
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

            CreatureState followPlayerState = new FollowPlayerState(CreatureData);
            CreatureState wanderState = new WanderState(CreatureData);
            CreatureState useConsumableState = new UseConsumableState(CreatureData);
            CreatureState attackPlayerState = new AttackPlayerState(CreatureData);

            // Wandering
            builder.In(followPlayerState).On(CreatureEvent.Event.LostPlayer).Goto(wanderState);

            // Follow player
            builder.In(wanderState).On(CreatureEvent.Event.SpottedPlayer).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);
            builder.In(followPlayerState).On(CreatureEvent.Event.SpottedPlayer).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);
            builder.In(useConsumableState).On(CreatureEvent.Event.RegainedHealthPlayerOutOfRange).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);
            builder.In(attackPlayerState).On(CreatureEvent.Event.PlayerOutOfRange).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);

            // Attack player
            builder.In(followPlayerState).On(CreatureEvent.Event.PlayerInRange).Goto(attackPlayerState).Execute<ICreatureData>(new AttackPlayerState(CreatureData).Do);
            builder.In(attackPlayerState).On(CreatureEvent.Event.PlayerInRange).Execute<ICreatureData>(new AttackPlayerState(CreatureData).Do);
            builder.In(useConsumableState).On(CreatureEvent.Event.RegainedHealthPlayerInRange).Goto(attackPlayerState).Execute<ICreatureData>(new AttackPlayerState(CreatureData).Do);

            // Use potion
            builder.In(attackPlayerState).On(CreatureEvent.Event.AlmostDead).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            builder.In(followPlayerState).On(CreatureEvent.Event.AlmostDead).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
    }
}
