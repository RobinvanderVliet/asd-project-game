using System.Threading;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;

namespace WorldGeneration.StateMachine
{
    public class MonsterStateMachine : DefaultStateMachine
    {
        private Timer _timer;

        public MonsterStateMachine(ICharacterData characterData, RuleSet ruleSet) : base (characterData, ruleSet)
        {
        }

        public ICharacterData CharacterData
        {
            get => _characterData;
            set => _characterData = (MonsterData)value;
        }

        public override void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, CreatureEvent.Event>();

            CreatureState followPlayerState = new FollowPlayerState(CharacterData);
            CreatureState wanderState = new WanderState(CharacterData);
            CreatureState useConsumableState = new UseConsumableState(CharacterData);
            CreatureState attackPlayerState = new AttackPlayerState(CharacterData);
            
            DefineDefaultBehaviour(ref builder, ref followPlayerState);
            DefineDefaultBehaviour(ref builder, ref wanderState);
            DefineDefaultBehaviour(ref builder, ref useConsumableState);
            DefineDefaultBehaviour(ref builder, ref attackPlayerState);

            // Wandering
            builder.In(followPlayerState).On(CreatureEvent.Event.LOST_PLAYER).Goto(wanderState);

            // Follow player
            builder.In(wanderState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICharacterData>(new FollowPlayerState(CharacterData).Do);
            builder.In(followPlayerState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICharacterData>(new FollowPlayerState(CharacterData).Do);
            builder.In(useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICharacterData>(new FollowPlayerState(CharacterData).Do);
            builder.In(attackPlayerState).On(CreatureEvent.Event.PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICharacterData>(new FollowPlayerState(CharacterData).Do);

            // Attack player
            builder.In(followPlayerState).On(CreatureEvent.Event.PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICharacterData>(new AttackPlayerState(CharacterData).Do);
            builder.In(attackPlayerState).On(CreatureEvent.Event.PLAYER_IN_RANGE).Execute<ICharacterData>(new AttackPlayerState(CharacterData).Do);
            builder.In(useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICharacterData>(new AttackPlayerState(CharacterData).Do);

            // Use potion
            builder.In(attackPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(new UseConsumableState(CharacterData).Do);
            builder.In(followPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(new UseConsumableState(CharacterData).Do);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
            
            // Start State machine loop
            Update();
        }
    }
}
