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
        public ICreature closestPlayer { get; set; }
        public Single distanceToClosestPlayer { get; set; }
        public ICreature closestMonster { get; set; }
        public Single distanceToClosestMonster { get; set; }

        public void ScanMap(Vector2 loc, int visionRange);
    }
}
