using System;
using System.Numerics;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class DataGatheringService : IDataGatheringService
    {
        public TrainerAI closestPlayer { get; set; }
        public Single distanceToClosestPlayer { get; set; } = 9999999999999999999;
        public TrainerAI closestMonster { get; set; }
        public Single distanceToClosestMonster { get; set; } = 9999999999999999999;

        private SmartMonster SmartMonster;

        public void ScanMap(SmartMonster smartMonster, int visionRange)
        {
            SetClosestMonster(smartMonster, visionRange);
            SetClosestPlayer(smartMonster, visionRange);
            this.SmartMonster = smartMonster;
        }

        private void SetClosestMonster(SmartMonster smartMonster, int visionRange)
        {
            foreach (TrainerAI monster in smartMonster.trainingMapGenerator.monsters)
            {
                Single distance = Vector2.Distance(smartMonster.creatureData.Position, monster.location);
                if (distance < visionRange && distance < distanceToClosestMonster)
                {
                    closestMonster = monster;
                    distanceToClosestMonster = distance;
                }
            }
        }

        private void SetClosestPlayer(SmartMonster smartMonster, int visionRange)
        {
            foreach (TrainerAI player in smartMonster.trainingMapGenerator.players)
            {
                Single distance = Vector2.Distance(smartMonster.creatureData.Position, player.location);
                if (distance < visionRange && distance < distanceToClosestPlayer)
                {
                    closestPlayer = player;
                    distanceToClosestPlayer = distance;
                }
            }
        }

        //For not smart AI update
        public SmartMonster ScanMapPlayerAI(Vector2 location, SmartMonster smartMonster)
        {
            if (Vector2.Distance(location, smartMonster.creatureData.Position) < 2)
            {
                return SmartMonster;
            }
            else
            {
                return null;
            }
        }

        public TrainerAI ScanMapMonsterAI(Vector2 location, SmartMonster smartMonster)
        {
            return SetClosestPlayerForAI(location, smartMonster);
        }

        private static TrainerAI SetClosestPlayerForAI(Vector2 location, SmartMonster smartMonster)
        {
            foreach (TrainerAI player in smartMonster.trainingMapGenerator.players)
            {
                Single distance = Vector2.Distance(location, player.location);
                if (distance == 1)
                {
                    return player;
                }
            }
            return null;
        }

        public void CheckNewPosition(SmartMonster smartMonster)
        {
            if (distanceToClosestPlayer < smartMonster.currDistanceToPlayer)
            {
                smartMonster.score = smartMonster.score + 10;
                smartMonster.currDistanceToPlayer = distanceToClosestPlayer;
            }
            else if (distanceToClosestPlayer > smartMonster.currDistanceToPlayer)
            {
                smartMonster.score = smartMonster.score - 10;
                smartMonster.currDistanceToPlayer = distanceToClosestPlayer;
            }
            if (distanceToClosestMonster < smartMonster.currDistanceToMonster)
            {
                smartMonster.score = smartMonster.score + 3;
                smartMonster.currDistanceToMonster = distanceToClosestMonster;
            }
            else if (distanceToClosestMonster > smartMonster.currDistanceToMonster)
            {
                smartMonster.score = smartMonster.score - 3;
                smartMonster.currDistanceToMonster = distanceToClosestMonster;
            }
        }
    }
}