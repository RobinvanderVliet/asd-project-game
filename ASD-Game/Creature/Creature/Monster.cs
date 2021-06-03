﻿using Creature.Creature.StateMachine;

namespace Creature.Creature
{
    public class Monster : ICreature
    {
        private ICreatureStateMachine _monsterStateMachine;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _monsterStateMachine;
        }

        public Monster(ICreatureStateMachine monsterStateMachine)
        {
            _monsterStateMachine = monsterStateMachine;
            _monsterStateMachine.StartStateMachine();
        }

        public void ApplyDamage(double amount)
        {
            _monsterStateMachine.CharacterData.Health -= amount;
        }

        public void HealAmount(double amount)
        {
            _monsterStateMachine.CharacterData.Health += amount;
        }
    }
}
