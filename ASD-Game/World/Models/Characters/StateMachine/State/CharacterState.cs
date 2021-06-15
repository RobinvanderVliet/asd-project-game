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
            
            Character visionRangeTarget = _characterData.WorldService.GetCharacterInClosestRangeToCurrentCharacter(_characterData.WorldService.GetCharacter(_characterData.CharacterId), VISION_RANGE);
            if (visionRangeTarget != null)
            {
                Character attackRangeTarget = _characterData.WorldService.GetCharacterInClosestRangeToCurrentCharacter(_characterData.WorldService.GetCharacter(_characterData.CharacterId), ATTACK_RANGE);
                if (attackRangeTarget != null)
                {
                    if (attackRangeTarget is Player)
                    {
                        var player = (Player)attackRangeTarget;
                        var agentData = new AgentData(player.XPosition, player.YPosition, 50);
                        _characterStateMachine.FireEvent(CharacterEvent.Event.CREATURE_IN_RANGE, agentData);
                    }
                    else if (attackRangeTarget is Monster)
                    {
                        Monster monster = (Monster)attackRangeTarget;
                        _characterStateMachine.FireEvent(CharacterEvent.Event.CREATURE_IN_RANGE, monster.MonsterData);
                    }
                }
                else
                {
                    if (visionRangeTarget is Player)
                    {
                        var player = (Player)visionRangeTarget;
                        var agentData = new AgentData(player.XPosition, player.YPosition, 50);
                        _characterStateMachine.FireEvent(CharacterEvent.Event.SPOTTED_CREATURE, agentData);
                    }
                    else if (visionRangeTarget is Monster)
                    {
                        Monster monster = (Monster)visionRangeTarget;
                        _characterStateMachine.FireEvent(CharacterEvent.Event.SPOTTED_CREATURE, monster.MonsterData);
                    } 
                    
                }
            }
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