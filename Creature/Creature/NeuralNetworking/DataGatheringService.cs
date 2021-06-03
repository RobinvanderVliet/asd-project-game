using System;
using System.Numerics;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class DataGatheringService
    {
        public ICreature closestPlayer { get; set; }
        public Single distanceToClosestPlayer { get; set; } = 9999999999999999999;
        public ICreature closestMonster { get; set; }
        public Single distanceToClosestMonster { get; set; } = 9999999999999999999;

        public void ScanMap(SmartMonster smartMonster, int visionRange)
        {
            SetClosestMonster(smartMonster, visionRange);
            SetClosestPlayer(smartMonster, visionRange);
        }

        private void SetClosestMonster(SmartMonster smartMonster, int visionRange)
        {
            //   foreach (ICreature monster in smartMonster.trainingMapGenerator.monsters)
            //   {
            //       Single distance = Vector2.Distance(smartMonster.creatureData.Position, monster.CreatureStateMachine.CreatureData.Position);
            //       if (distance < visionRange && distance < distanceToClosestMonster)
            //       {
            //           closestMonster = monster;
            //           distanceToClosestMonster = distance;
            //       }
            //   }
        }

        private void SetClosestPlayer(SmartMonster smartMonster, int visionRange)
        {
            //   foreach (ICreature player in smartMonster.trainingMapGenerator.players)
            //   {
            //       Single distance = Vector2.Distance(smartMonster.creatureData.Position, player.CreatureStateMachine.CreatureData.Position);
            //       if (distance < visionRange && distance < distanceToClosestPlayer)
            //       {
            //           closestPlayer = player;
            //           distanceToClosestPlayer = distance;
            //       }
            //   }
        }

        public void CheckNewPosition(SmartMonster smartMonster)
        {
            if (distanceToClosestPlayer < smartMonster.currDistanceToPlayer)
            {
                smartMonster.currDistanceToPlayer = distanceToClosestPlayer;
            }
            else if (distanceToClosestPlayer > smartMonster.currDistanceToPlayer)
            {
                smartMonster.currDistanceToPlayer = distanceToClosestPlayer;
            }
            if (distanceToClosestMonster < smartMonster.currDistanceToMonster)
            {
                smartMonster.currDistanceToMonster = distanceToClosestMonster;
            }
            else if (distanceToClosestMonster > smartMonster.currDistanceToMonster)
            {
                smartMonster.currDistanceToMonster = distanceToClosestMonster;
            }
        }
    }
}