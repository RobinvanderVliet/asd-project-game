using System;
using System.Collections.Generic;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class DataGatheringService : IDataGatheringService
    {

        public ICreature closestPlayer { get; set; }
        public Single distanceToClosestPlayer { get; set; } = 0;
        public ICreature closestMonster { get; set; }
        public Single distanceToClosestMonster { get; set; } = 0;
        public TrainingMapGenerator trainingMapGenerator = new TrainingMapGenerator();
        //Closest Item

        public void ScanMap(Vector2 loc, int visionRange) 
        {
            SetClosestMonster(loc, visionRange);
            SetClosestPlayer(loc, visionRange);
        }

        private void SetClosestMonster(Vector2 loc, int visionRange) 
        {
            foreach(ICreature monster in trainingMapGenerator.monsters)
            {
                Single distance = Vector2.Distance(loc, monster.CreatureStateMachine.CreatureData.Position);
                if(distance < visionRange && distance < distanceToClosestMonster)
                {
                    closestMonster = monster;
                    distanceToClosestMonster = distance;
                }
            }
        }

        private void SetClosestPlayer(Vector2 loc, int visionRange) 
        {
            foreach (ICreature player in trainingMapGenerator.players)
            {
                Single distance = Vector2.Distance(loc, player.CreatureStateMachine.CreatureData.Position);
                if (distance < visionRange && distance < distanceToClosestMonster)
                {
                    closestMonster = player;
                    distanceToClosestMonster = distance;
                }
            }
        }
    }
}
