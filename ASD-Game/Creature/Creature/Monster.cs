using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;
using WorldGeneration;

namespace Creature.Creature
{
    public class Monster : Character
    {
        public ICreatureStateMachine MonsterStateMachine { get; set; }
        public MonsterData MonsterData { get; set; }

        public Monster(string name, int xPosition, int yPosition, string symbol) : base(name, xPosition, yPosition, symbol)
        {
            SetStats(0);
            MonsterStateMachine = new MonsterStateMachine(MonsterData);
        }

        private void SetStats(int difficulty)
        {
            CreateMonsterData(difficulty);
        }

        private void CreateMonsterData(int difficulty)
        {
            MonsterData = new MonsterData(XPosition, YPosition, difficulty);
        }
    }
}
