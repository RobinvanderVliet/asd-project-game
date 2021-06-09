using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ASD_Game.World.Models.Characters.StateMachine.Data
{
    [ExcludeFromCodeCoverage]
    public class MonsterData : ICharacterData
    {
        private Vector2 _position;
        private double _health = 40;
        private int _damage = 10;
        private int _visionRange = 6;

        public bool IsAlive { get => _health > 0; }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public double Health
        {
            get => _health;
            set => _health = value;
        }

        public IWorld World { get; set; }

        public int Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public int VisionRange
        {
            get => _visionRange;
            set => _visionRange = value;
        }

        public MonsterData(int xPos, int yPos, int difficulty)
        {
            _position = new Vector2(xPos, yPos);
            SetStats(difficulty);
        }

        private void SetStats(int diff)
        {
            switch (diff)
            {
                case 0:
                    Health = Health / 2;
                    Damage = Damage / 2;
                    break;

                case 50:
                    Health = Health;
                    Damage = Damage;
                    break;

                case 100:
                    Health = Health * 2;
                    Damage = Damage * 2;
                    break;
            }
        }
    }
}