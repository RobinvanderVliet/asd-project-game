using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using WorldGeneration.StateMachine;

namespace Creature.Creature.StateMachine.State
{
    public class AttackState : CharacterState
    {
        public AttackState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(characterData, characterStateMachine)
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
                        if (_characterData is MonsterData)
                        {
                            AIAttack();
                        }
                        else
                        {
                            AgentAttack();
                        }
                    }
                }
            }
        }

        private void AIAttack()
        {
            _characterData.Destination = _target.Position;
            _characterData.MoveType = "Attack";
        }

        private void AgentAttack()
        {
            _characterData.AttackHandler.SendAttack(GetDirection());
        }

        private string GetDirection()
        {
            float PX = _characterData.Position.X;
            float PY = _characterData.Position.Y;
            float TX = _target.Position.X;
            float TY = _target.Position.Y;

            if (PX == TX && PY > TY) { return "down"; }
            if (PX > TX && PY == TY) { return "right"; }
            if (PX == TX && PY < TY) { return "up"; }
            if (PX < TX && PY == TY) { return "left"; }
            return null;
        }
    }
}