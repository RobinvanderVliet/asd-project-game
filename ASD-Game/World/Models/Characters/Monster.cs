using World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine;

namespace WorldGeneration
{
    public class Monster : Character
    {
        public ICharacterStateMachine MonsterStateMachine { get; set; }
        public MonsterData MonsterData { get; set; }

        public Monster(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol, id)
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
            MonsterData = new MonsterData(
                XPosition,
                YPosition,
                difficulty);
        }
    }
}