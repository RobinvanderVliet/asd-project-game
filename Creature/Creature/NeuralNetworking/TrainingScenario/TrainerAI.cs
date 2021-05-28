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
        private readonly DataGatheringService dataGatheringService;
        private SmartMonster _target;

        [ExcludeFromCodeCoverage]
        public TrainerAI(Vector2 loc, string type)
        {
            this.damage = random.Next(5, 10);
            this.health = random.Next(10, 50);
            this.location = loc;
            this.type = type;
            this.dataGatheringService = new DataGatheringService();
        }

        public void update(SmartMonster smartMonster)
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

        public bool Adjecent(SmartMonster smartMonster)
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

        private void Attack(SmartMonster smartMonster)
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
            if (location.X < 28 && location.Y < 28 && location.X > 2 && location.Y > 2)
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