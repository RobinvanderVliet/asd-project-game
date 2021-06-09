using System;
using ASD_Game.World.Models.Characters.StateMachine.Data;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public abstract class CharacterState : IComparable
    {
        protected ICharacterData _characterData;

        public CharacterState(ICharacterData characterData)
        {
            _characterData = characterData;
        }

        public virtual void Entry()
        {
            throw new NotImplementedException();
        }

        public virtual void Do()
        {
            throw new NotImplementedException();
        }

        public virtual void Do(ICharacterData characterData)
        {
            throw new NotImplementedException();
        }

        public virtual void Exit()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new InvalidOperationException("State machine is not a comparable object.");
        }
    }
}