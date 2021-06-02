using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class TrainerAI
    {
        public string type;
        public int damage;
        public int health;

        public Vector2 location;

        private readonly Random random = new Random();
        private readonly DataGatheringServiceForTraining dataGatheringService;
        private SmartMonsterForTraining _target;

        public TrainerAI(Vector2 loc, string type)
        {
            this.damage = random.Next(5, 10);
            this.health = random.Next(10, 50);
            this.location = loc;
            this.type = type;
            this.dataGatheringService = new DataGatheringServiceForTraining();
        }

        public void update(SmartMonsterForTraining smartMonster)
        {
            if (Adjecent(smartMonster))
            {
                Attack(_target);
            }
            else
            {
                Walk();
            }
        }

        private bool Adjecent(SmartMonsterForTraining smartMonster)
        {
            TrainerAI _monsterTarget;
            if (type.Equals("player"))
            {
                _target = dataGatheringService.ScanMapPlayerAI(location, smartMonster);
                if (_target != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                _monsterTarget = dataGatheringService.ScanMapMonsterAI(location, smartMonster);
                if (_monsterTarget != null)
                {
                    return true;
                }
                return false;
            }
        }

        private void Attack(SmartMonsterForTraining smartMonster)
        {
            if (type.Equals("player"))
            {
                smartMonster.smartactions.TakeDamage(damage, _target);
            }
            else
            {
                Walk();
            }
        }

        private void Walk()
        {
            if (location.X <= 29 && location.Y <= 29 && location.X >= 1 && location.Y >= 1)
            {
                int direction = random.Next(1, 2);
                if (direction < 2)
                {
                    int direction2 = random.Next(1, 2);
                    if (direction2 < 2)
                    {
                        location.Y--;
                    }
                    else
                    {
                        location.Y++;
                    }
                }
                else
                {
                    int direction3 = random.Next(1, 2);
                    if (direction3 < 2)
                    {
                        location.X--;
                    }
                    else
                    {
                        location.X++;
                    }
                }
            }
        }
    }
}