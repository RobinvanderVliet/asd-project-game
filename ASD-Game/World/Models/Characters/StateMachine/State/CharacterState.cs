using System;

using ASD_Game.World.Models.Characters.StateMachine.Data;
using World.Models.Characters.StateMachine.Event;

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
            const int ATTACK_RANGE = 1;
            const int VISION_RANGE = 6;

            //if (_characterData.WorldService.GetItemsOnCurrentTileWithPlayerId(_characterData.CharacterId) != null)
            //{
            //    _characterStateMachine.FireEvent(CharacterEvent.Event.FOUND_ITEM);
            //}
            //else
            //{
            Character visionRangeTarget = _characterData.WorldService.GetCharacterInClosestRangeToCurrentCharacter(_characterData.WorldService.GetCharacter(_characterData.CharacterId), VISION_RANGE);
            if (visionRangeTarget != null)
            {
                Character attackRangeTarget = _characterData.WorldService.GetCharacterInClosestRangeToCurrentCharacter(_characterData.WorldService.GetCharacter(_characterData.CharacterId), ATTACK_RANGE);
                if (attackRangeTarget != null)
                {
                    //if (attackRangeTarget is World.Models.Characters.Agent)
                    //_characterStateMachine.FireEvent(CharacterEvent.Event.CREATURE_IN_RANGE, (Player)attackRangeTarget.);
                    if (attackRangeTarget is Monster)
                    {
                        Monster monster = (Monster)attackRangeTarget;
                        _characterStateMachine.FireEvent(CharacterEvent.Event.CREATURE_IN_RANGE, monster.MonsterData);
                    }
                }
                else
                {
                    if (visionRangeTarget is Monster)
                    {
                        Monster monster = (Monster)visionRangeTarget;
                        _characterStateMachine.FireEvent(CharacterEvent.Event.SPOTTED_CREATURE, monster.MonsterData);
                    }
                }
            }
            else
            {
                //_characterStateMachine.FireEvent(CharacterEvent.Event.LOST_CREATURE);
            }
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