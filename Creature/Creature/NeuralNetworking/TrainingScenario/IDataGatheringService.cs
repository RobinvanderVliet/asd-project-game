using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public interface IDataGatheringService
    {
        public TrainerAI closestPlayer { get; set; }
        public Single distanceToClosestPlayer { get; set; }
        public TrainerAI closestMonster { get; set; }
        public Single distanceToClosestMonster { get; set; }

        public void ScanMap(SmartMonster smartMonster, int visionRange);

        public void CheckNewPosition(SmartMonster smartMonster);
    }
}
