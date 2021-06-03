﻿using Creature.Creature.StateMachine.Data;
using System;

namespace Creature.Creature.StateMachine.State
{
    public abstract class CreatureState : IComparable
    {
        protected ICharacterData _characterData;

        public CreatureState(ICharacterData characterData)
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