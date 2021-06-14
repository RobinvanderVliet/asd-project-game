using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Appccelerate.StateMachine.Machine;
using ASD_Game.World.Models.Characters.StateMachine.CustomRuleSet;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using WorldGeneration.StateMachine.Data;
using WorldGeneration.StateMachine.Event;

namespace ASD_Game.World.Models.Characters.StateMachine
{
    public class MonsterStateMachine : DefaultStateMachine
    {
        private Timer _timer;

        public MonsterStateMachine(ICharacterData characterData, RuleSet ruleSet) : base(characterData, ruleSet)
        {
        }

        [ExcludeFromCodeCoverage]
        public ICharacterData CharacterData
        {
            get => _characterData;
            set => _characterData = (MonsterData)value;
        }

        public override void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CharacterState, CharacterEvent.Event>();

            CharacterState followPlayerState = new FollowPlayerState(CharacterData);
            CharacterState wanderState = new WanderState(CharacterData);
            CharacterState useConsumableState = new UseConsumableState(CharacterData);
            CharacterState attackPlayerState = new AttackPlayerState(CharacterData);

            DefineDefaultBehaviour(ref builder, ref followPlayerState);
            DefineDefaultBehaviour(ref builder, ref wanderState);
            DefineDefaultBehaviour(ref builder, ref useConsumableState);
            DefineDefaultBehaviour(ref builder, ref attackPlayerState);

            // Wandering
            builder.In(followPlayerState).On(CharacterEvent.Event.LOST_PLAYER).Goto(wanderState);

            // Follow player
            builder.In(wanderState).On(CharacterEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICharacterData>(new FollowPlayerState(CharacterData).Do);
            builder.In(followPlayerState).On(CharacterEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICharacterData>(new FollowPlayerState(CharacterData).Do);
            builder.In(useConsumableState).On(CharacterEvent.Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICharacterData>(new FollowPlayerState(CharacterData).Do);
            builder.In(attackPlayerState).On(CharacterEvent.Event.PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICharacterData>(new FollowPlayerState(CharacterData).Do);

            // Attack player
            builder.In(followPlayerState).On(CharacterEvent.Event.PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICharacterData>(new AttackPlayerState(CharacterData).Do);
            builder.In(attackPlayerState).On(CharacterEvent.Event.PLAYER_IN_RANGE).Execute<ICharacterData>(new AttackPlayerState(CharacterData).Do);
            builder.In(useConsumableState).On(CharacterEvent.Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICharacterData>(new AttackPlayerState(CharacterData).Do);

            // Use potion
            builder.In(attackPlayerState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(new UseConsumableState(CharacterData).Do);
            builder.In(followPlayerState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(new UseConsumableState(CharacterData).Do);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();

            // Start State machine loop
            Update();
        }
    }
}