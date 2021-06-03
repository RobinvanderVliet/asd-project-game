using System.Diagnostics.CodeAnalysis;
using WorldGeneration.StateMachine.Data;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    [ExcludeFromCodeCoverage]
    public class TrainingScenario
    {
        public Population pop;
        public Genome bestGene;
        public bool runTraining = false;
        public bool runOnce = false;

        public void StartTraining()
        {
            SetupTraining();
            RunTrainingScenarios();
        }

        public void SetupTraining()
        {
            MonsterData data =
            new MonsterData
            (
                14,
                14,
                0
            );
            pop = new Population(50, data);
            runTraining = true;
        }

        public void ContinueTraining(Genome gene)
        {
            MonsterData data =
            new MonsterData
            (
                14,
                14,
                0
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
                    if (pop.bestScore >= 100)
                    {
                        bestGene = pop.bestSmartMonster.brain;
                    }
                }
                runOnce = false;
            }
        }

        public Genome brainTransplant()
        {
            return bestGene;
        }

        public MonsterData SetMonsterData()
        {
            MonsterData data =
            new MonsterData
            (
            14,
            14,
            0
            );
            return data;
        }
    }
}