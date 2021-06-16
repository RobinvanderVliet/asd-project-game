using ASD_Game.World.Models.Characters.StateMachine;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using System.Numerics;

namespace ASD_Game.World.Models.Characters
{
    public class Monster : Character
    {
        public ICharacterStateMachine MonsterStateMachine { get; set; }
        public MonsterData MonsterData { get; set; }
        public Vector2 Destination { get; set; }
        public string MoveType { get; set; }

        public Monster(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol, id)
        {
            SetStats(1);
            MonsterStateMachine = new MonsterStateMachine(MonsterData);
        }

        public void Update()
        {
            Destination = MonsterData.Destination;
            MoveType = MonsterData.MoveType;
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
        
        public override int GetDamage()
        {
            return MonsterData.Damage;
        }
    }
}