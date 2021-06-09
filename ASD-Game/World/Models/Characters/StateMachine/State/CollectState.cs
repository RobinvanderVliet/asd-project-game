using World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine.State;

namespace Creature.Creature.StateMachine.State
{
    public class CollectState : CharacterState
    {
        public CollectState(ICharacterData characterData) : base(characterData)
        {
        }

        public override void Do()
        {
            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;
            
            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "collect")
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