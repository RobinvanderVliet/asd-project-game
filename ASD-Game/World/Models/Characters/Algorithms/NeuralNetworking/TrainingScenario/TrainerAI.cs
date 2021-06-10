using System;
using System.Numerics;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario
{
    public class TrainerAI
    {
        public string Type;
        public int Damage;
        public int Health;

        public Vector2 Location;

        private readonly Random _random = new Random();
        private readonly DataGatheringServiceForTraining _dataGatheringService;
        private SmartMonsterForTraining _target;

        public TrainerAI(Vector2 loc, string type)
        {
            Damage = _random.Next(1, 10);
            Health = _random.Next(1, 50);
            Location = loc;
            Type = type;
            _dataGatheringService = new DataGatheringServiceForTraining();
        }

        public void Update(SmartMonsterForTraining smartMonster)
        {
            if (Adjacent(smartMonster))
            {
                Attack(_target);
            }
            else
            {
                Walk();
            }
        }

        private bool Adjacent(SmartMonsterForTraining smartMonster)
        {
            TrainerAI _monsterTarget;
            if (Type.Equals("player"))
            {
                _target = _dataGatheringService.ScanMapPlayerAI(Location, smartMonster);
                if (_target != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                _monsterTarget = _dataGatheringService.ScanMapMonsterAI(Location, smartMonster);
                if (_monsterTarget != null)
                {
                    return true;
                }
                return false;
            }
        }

        private void Attack(SmartMonsterForTraining smartMonster)
        {
            if (Type.Equals("player"))
            {
                smartMonster.Smartactions.TakeDamage(Damage, _target);
            }
            else
            {
                Walk();
            }
        }

        private void Walk()
        {
            if (Location.X <= 29 && Location.Y <= 29 && Location.X >= 1 && Location.Y >= 1)
            {
                int direction = _random.Next(1, 2);
                if (direction < 2)
                {
                    int direction2 = _random.Next(1, 2);
                    if (direction2 < 2)
                    {
                        Location.Y--;
                    }
                    else
                    {
                        Location.Y++;
                    }
                }
                else
                {
                    int direction3 = _random.Next(1, 2);
                    if (direction3 < 2)
                    {
                        Location.X--;
                    }
                    else
                    {
                        Location.X++;
                    }
                }
            }
        }
    }
}