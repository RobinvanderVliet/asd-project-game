using World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine;
using WorldGeneration.StateMachine.State;

namespace Creature.Creature.StateMachine.State
{
    public class UseConsumableState : CharacterState
    {
        public UseConsumableState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(characterData, characterStateMachine)
        {
        }

        public override void Do()
        {
            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;
            
            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "attack")
                {
                    if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
                    {
                        //TODO implement Attack logic + gather targetData
                    }
                }
            }
        }
    }
}