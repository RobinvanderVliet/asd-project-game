using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using WorldGeneration.StateMachine;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class IdleState : CharacterState
    {
        public IdleState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(characterData, characterStateMachine)
        {
            
        }

        public void Do()
        {
            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;
            
            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "idle")
                {
                    if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
                    {
                        //Idle state is meant for the AI to be able to do nothing, but check if it needs to do something. in Order to save CPU utilization
                    }
                }
            }
        }
    }
}