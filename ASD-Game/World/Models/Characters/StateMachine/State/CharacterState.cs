using System;
using ASD_Game.World.Models.Characters.StateMachine.Data;


namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public abstract class CharacterState : IComparable
    {
        protected ICharacterData _characterData;
        protected ICharacterData _target;

        public CharacterState(ICharacterData characterData)
        {
            _characterData = characterData;
        }

        public virtual void Do()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new InvalidOperationException("State machine is not a comparable object.");
        }
        
        public virtual void SetTargetData(ICharacterData data)
        {
            _target = data;
        }
    }
}