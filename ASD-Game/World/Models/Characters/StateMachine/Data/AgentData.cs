using System.Collections.Generic;
using System.Numerics;
using ActionHandling;
using ASD_Game.ActionHandling;
using ASD_Game.World.Models.Characters.StateMachine.Builder;
using ASD_Game.World.Services;

namespace ASD_Game.World.Models.Characters.StateMachine.Data
{
    public class AgentData : ICharacterData
    {
        private Vector2 _position;
        private double _health = 40;
        private int _damage = 10;
        private int _visionRange = 6;

        public bool IsAlive
        {
            get => _health > 0;
        }

        public int Stamina { get; set; }

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

        public List<KeyValuePair<string, string>> RuleSet { get; set; }
        public Inventory Inventory { get; set; }
        public int Team { get; set; }
        public int RadiationLevel { get; set; }
        public IMoveHandler MoveHandler { get; set; }
        public IWorldService WorldService { get; set; }
        public BuilderConfigurator BuilderConfigurator { get; set; }

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

        public IAttackHandler AttackHandler { get; set; }
        public string CharacterId { get; set; }

        public AgentData(int xPos, int yPos, int difficulty)
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