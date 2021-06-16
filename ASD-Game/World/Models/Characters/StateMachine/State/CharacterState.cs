using System;
using ASD_Game.World.Models.Characters.StateMachine.CustomRuleSet;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using World.Models.Characters.StateMachine.Event;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public abstract class CharacterState : IComparable
    {
        private const int ATTACK_RANGE = 1;
        private const int VISION_RANGE = 6;
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
            if (_characterData is AgentData)
            {
                Console.Write(_characterData.Health);
            }

            Character visionRangeTarget = GetClosestCharacterInRange(VISION_RANGE);

            if (visionRangeTarget is SmartMonster) return;

            if (visionRangeTarget != null)
            {
                if (visionRangeTarget is Player && !ThresholdExists("player"))
                {
                    _characterStateMachine.FireEvent(CharacterEvent.Event.LOST_CREATURE);
                    return;
                }

                if (visionRangeTarget is Monster && !ThresholdExists("monster"))
                {
                    _characterStateMachine.FireEvent(CharacterEvent.Event.LOST_CREATURE);
                    return;
                }

                Character attackRangeTarget = GetClosestCharacterInRange(ATTACK_RANGE);
                if (attackRangeTarget is SmartMonster) return;
                
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
            else
            {
                _characterStateMachine.FireEvent(CharacterEvent.Event.LOST_CREATURE);
            }
        }

        private Character GetClosestCharacterInRange(int range)
        {
            Character closestCharacter = null;
            if (ThresholdExists("player"))
            {
                closestCharacter =
                    _characterData.WorldService.GetPlayerInClosestRangeToCurrentCharacter(
                        _characterData.WorldService.GetCharacter(_characterData.CharacterId), range);
            }
            if (ThresholdExists("monster") && closestCharacter == null)
            {
                closestCharacter =
                    _characterData.WorldService.GetMonsterInClosestRangeToCurrentCharacter(
                        _characterData.WorldService.GetCharacter(_characterData.CharacterId), range);
            }

            return closestCharacter;
        }

        public int CompareTo(object obj)
        {
            throw new InvalidOperationException("State machine is not a comparable object.");
        }

        public virtual void SetTargetData(ICharacterData data)
        {
            _target = data;
        }

        private bool ThresholdExists(string threshold)
        {
            var infoList = _characterData.BuilderConfigurator.GetBuilderInfoList();

            foreach (var info in infoList)
            {
                var result = info.RuleSets.Find(ruleset => ruleset.Threshold == threshold);
                if (result != null) return true;
            }

            return false;
        }
    }
}