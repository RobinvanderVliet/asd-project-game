using System;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public interface IDataGatheringService
    {
        public TrainerAI closestPlayer { get; set; }
        public Single distanceToClosestPlayer { get; set; }
        public TrainerAI closestMonster { get; set; }
        public Single distanceToClosestMonster { get; set; }

        public void ScanMap(SmartMonsterForTraining smartMonster, int visionRange);

        public void CheckNewPosition(SmartMonsterForTraining smartMonster);
    }
}