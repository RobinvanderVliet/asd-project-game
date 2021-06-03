using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class DataGatheringServiceForTraining
    {
        public TrainerAI closestPlayer { get; set; }
        public Single distanceToClosestPlayer { get; set; } = 9999999999999999999;
        public TrainerAI closestMonster { get; set; }
        public Single distanceToClosestMonster { get; set; } = 9999999999999999999;

        public void ScanMap(SmartMonsterForTraining smartMonster, int visionRange)
        {
            SetClosestMonster(smartMonster, visionRange);
            SetClosestPlayer(smartMonster, visionRange);
        }

        private void SetClosestMonster(SmartMonsterForTraining smartMonster, int visionRange)
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

        private void SetClosestPlayer(SmartMonsterForTraining smartMonster, int visionRange)
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
        public SmartMonsterForTraining ScanMapPlayerAI(Vector2 location, SmartMonsterForTraining smartMonster)
        {
            if (Vector2.Distance(location, smartMonster.creatureData.Position) < 2)
            {
                return smartMonster;
            }
            else
            {
                return null;
            }
        }

        [ExcludeFromCodeCoverage]
        public TrainerAI ScanMapMonsterAI(Vector2 location, SmartMonsterForTraining smartMonster)
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

        public void CheckNewPosition(SmartMonsterForTraining smartMonster)
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