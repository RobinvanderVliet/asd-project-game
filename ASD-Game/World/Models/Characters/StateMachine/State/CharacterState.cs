using System;

using ASD_Game.World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine;


namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public abstract class CharacterState : IComparable
    {
        protected ICharacterData _characterData;
        protected ICharacterData _target;
        protected ICharacterStateMachine _characterStateMachine;

        public CharacterState(ICharacterData characterData, ICharacterStateMachine characterStateMachine)
        {
            _characterData = characterData;
            _characterStateMachine = characterStateMachine;
        }

        public virtual void Do()
        {
            throw new NotImplementedException();
        }

        public virtual void DoWorldCheck()
        {
            int attackRange = 3;
            int visionRange = 6;

            //if (_characterData.WorldService.GetItemsOnCurrentTileWithPlayerId(_characterData.CharacterId) != null)
            //{
            //    _characterStateMachine.FireEvent(CharacterEvent.Event.FOUND_ITEM);
            //}
            //else
            //{
            //    if (Vector2.DistanceSquared(new Vector2(_characterData.Position.X, _characterData.Position.Y), new Vector2(_target.Position.X, _target.Position.Y)) <= visionRange)
            //    {

            //        if (Vector2.DistanceSquared(new Vector2(_characterData.Position.X, _characterData.Position.Y), new Vector2(_target.Position.X, _target.Position.Y)) <= attackRange)
            //        {
            //            _characterStateMachine.FireEvent(CharacterEvent.Event.CREATURE_IN_RANGE);
            //        }
            //        else
            //        {
            //            _characterStateMachine.FireEvent(CharacterEvent.Event.SPOTTED_CREATURE);
            //        }
            //    }
            //    else
            //    {
            //        _characterStateMachine.FireEvent(CharacterEvent.Event.LOST_CREATURE);
            //    }
            //}
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