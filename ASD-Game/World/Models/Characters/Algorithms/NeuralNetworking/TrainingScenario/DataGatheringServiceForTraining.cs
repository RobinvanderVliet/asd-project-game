using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario
{
    public class DataGatheringServiceForTraining
    {
        public TrainerAI ClosestPlayer { get; set; }
        public Single DistanceToClosestPlayer { get; set; } = 9999999999999999999;
        public TrainerAI ClosestMonster { get; set; }
        public Single DistanceToClosestMonster { get; set; } = 9999999999999999999;

        public void ScanMap(SmartMonsterForTraining smartMonster, int visionRange)
        {
            SetClosestMonster(smartMonster, visionRange);
            SetClosestPlayer(smartMonster, visionRange);
        }

        private void SetClosestMonster(SmartMonsterForTraining smartMonster, int visionRange)
        {
            foreach (TrainerAI monster in smartMonster.TrainingMapGenerator.monsters)
            {
                Single distance = Vector2.Distance(smartMonster.CreatureData.Position, monster.Location);
                if (distance < visionRange && distance < DistanceToClosestMonster)
                {
                    ClosestMonster = monster;
                    DistanceToClosestMonster = distance;
                }
            }
        }

        private void SetClosestPlayer(SmartMonsterForTraining smartMonster, int visionRange)
        {
            foreach (TrainerAI player in smartMonster.TrainingMapGenerator.players)
            {
                Single distance = Vector2.Distance(smartMonster.CreatureData.Position, player.Location);
                if (distance < visionRange && distance < DistanceToClosestPlayer)
                {
                    ClosestPlayer = player;
                    DistanceToClosestPlayer = distance;
                }
            }
        }

        //For not smart AI update
        public SmartMonsterForTraining ScanMapPlayerAI(Vector2 location, SmartMonsterForTraining smartMonster)
        {
            if (Vector2.Distance(location, smartMonster.CreatureData.Position) < 2)
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
            foreach (TrainerAI player in smartMonster.TrainingMapGenerator.players)
            {
                Single distance = Vector2.Distance(location, player.Location);
                if (distance == 1)
                {
                    return player;
                }
            }
            return null;
        }

        public void CheckNewPosition(SmartMonsterForTraining smartMonster)
        {
            if (DistanceToClosestPlayer < smartMonster.CurrDistanceToPlayer)
            {
                smartMonster.Score = smartMonster.Score + 10;
                smartMonster.CurrDistanceToPlayer = DistanceToClosestPlayer;
            }
            else if (DistanceToClosestPlayer > smartMonster.CurrDistanceToPlayer)
            {
                smartMonster.Score = smartMonster.Score - 10;
                smartMonster.CurrDistanceToPlayer = DistanceToClosestPlayer;
            }
            if (DistanceToClosestMonster < smartMonster.CurrDistanceToMonster)
            {
                smartMonster.Score = smartMonster.Score + 3;
                smartMonster.CurrDistanceToMonster = DistanceToClosestMonster;
            }
            else if (DistanceToClosestMonster > smartMonster.CurrDistanceToMonster)
            {
                smartMonster.Score = smartMonster.Score - 3;
                smartMonster.CurrDistanceToMonster = DistanceToClosestMonster;
            }
        }
    }
}