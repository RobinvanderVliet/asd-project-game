using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using System;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class AttackState : CharacterState
    {
        public AttackState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(characterData, characterStateMachine)
        {
            
        }
        
        public override void Do()
        {
            DoWorldCheck();

            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;

            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "attack")
                {
                    if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
                    {
                        //TODO implement Attack logic +gather targetData

                        Console.WriteLine("Player health: " + _characterData.WorldService.GetCharacter(_characterData.CharacterId).Health);

                        _target.AttackHandler.SendAttack("");
                        _target.Health -= 5;
                        Console.WriteLine("Enemy health: " + _target.Health);

                    }
                }
            }
        }
    }
}