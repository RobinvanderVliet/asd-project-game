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

        private static void Main(string[] args)
        {
            TrainingScenario scenario = new TrainingScenario();
            scenario.SetupTraining();

            scenario.RunTestScenarios();
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

        public void RunTestScenarios()
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