using Creature.Creature.NeuralNetworking;
using System;
using WorldGeneration.StateMachine;
using WorldGeneration.StateMachine.Data;

namespace WorldGeneration
{
    public class Monster : Character
    {
        public ICharacterStateMachine _monsterStateMachine;
        public MonsterData _monsterData;
        public Genome brain;

        public ICharacterStateMachine CharacterStateMachine
        {
            get => _monsterStateMachine;
        }

        public Monster(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol, id)
        {
            SetStats(0);
            if (_monsterStateMachine != null)
            {
                _monsterStateMachine.StartStateMachine();
            }
        }

        private void SetStats(int difficulty)
        {
            CreateMonsterData(difficulty);
        }

        private void CreateMonsterData(int difficulty)
        {
            _monsterData = new MonsterData(
                XPosition,
                YPosition,
                difficulty);
        }
    }
}