using ASD_Game.World.Models.Characters.StateMachine;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine;
using WorldGeneration.StateMachine.Data;

namespace ASD_Game.World.Models.Characters
{
    public class Monster : Character
    {
        public ICharacterStateMachine MonsterStateMachine;
        public MonsterData MonsterData;

        public Monster(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol, id)
        {
            SetStats(0);
            MonsterStateMachine = new MonsterStateMachine(MonsterData, null);
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