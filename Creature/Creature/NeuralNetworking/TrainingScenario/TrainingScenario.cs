using Creature.Creature.StateMachine.Data;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    [ExcludeFromCodeCoverage]
    public class TrainingScenario
    {
        public Population pop;
        public bool runTraining = false;
        public bool runOnce = false;

        public TrainingScenario()
        {
            SetupTraining();
            RunTrainingScenarios();
        }

        public void SetupTraining()
        {
            MonsterData data =
            new MonsterData
            (
                new Vector2(14, 14),
                20,
                5,
                200,
                null,
                false
            );
            pop = new Population(50, data);
            runTraining = true;
        }

        public void ContinueTraining(Genome gene)
        {
            MonsterData data =
            new MonsterData
            (
                new Vector2(14, 14),
                20,
                5,
                200,
                null,
                false
            );
            pop = new Population(50, data, gene);
            runTraining = true;
        }

        public void RunTrainingScenarios()
        {
            while (runTraining || runOnce)
            {
                if (!pop.Done())
                {
                    //if any players are alive then update them
                    pop.UpdateAlive();
                }
                else
                {
                    //all dead
                    //genetic algorithm
                    pop.NaturalSelection();
                }
                runOnce = false;
            }
        }
    }
}