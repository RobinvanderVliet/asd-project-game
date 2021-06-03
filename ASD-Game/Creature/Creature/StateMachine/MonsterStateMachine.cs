using System.Threading;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;

namespace Creature.Creature.StateMachine
{
    public class MonsterStateMachine : DefaultStateMachine
    {
        private Timer _timer;

        public MonsterStateMachine(ICreatureData creatureData, RuleSet ruleSet) : base (creatureData, ruleSet)
        {
        }

        public ICreatureData CreatureData
        {
            get => _creatureData;
            set => _creatureData = (MonsterData)value;
        }

        public override void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, CreatureEvent.Event>();

            CreatureState followPlayerState = new FollowPlayerState(CreatureData);
            CreatureState wanderState = new WanderState(CreatureData);
            CreatureState useConsumableState = new UseConsumableState(CreatureData);
            CreatureState attackPlayerState = new AttackPlayerState(CreatureData);
            
            DefineDefaultBehaviour(ref builder, ref followPlayerState);
            DefineDefaultBehaviour(ref builder, ref wanderState);
            DefineDefaultBehaviour(ref builder, ref useConsumableState);
            DefineDefaultBehaviour(ref builder, ref attackPlayerState);

            // Wandering
            builder.In(followPlayerState).On(CreatureEvent.Event.LOST_PLAYER).Goto(wanderState);

            // Follow player
            builder.In(wanderState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);
            builder.In(followPlayerState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);
            builder.In(useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);
            builder.In(attackPlayerState).On(CreatureEvent.Event.PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICreatureData>(new FollowPlayerState(CreatureData).Do);

            // Attack player
            builder.In(followPlayerState).On(CreatureEvent.Event.PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICreatureData>(new AttackPlayerState(CreatureData).Do);
            builder.In(attackPlayerState).On(CreatureEvent.Event.PLAYER_IN_RANGE).Execute<ICreatureData>(new AttackPlayerState(CreatureData).Do);
            builder.In(useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICreatureData>(new AttackPlayerState(CreatureData).Do);

            // Use potion
            builder.In(attackPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            builder.In(followPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
            
            // Start State machine loop
            Update();
        }
    }
}
