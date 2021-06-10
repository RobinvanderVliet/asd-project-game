using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Characters.StateMachine.Data;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario
{
    [ExcludeFromCodeCoverage]
    public class TrainingScenario
    {
        public Population Pop;
        public Genome BestGene;
        public bool RunTraining = false;
        public bool RunOnce = false;

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
            Pop = new Population(50, data);
            RunTraining = true;
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
            Pop = new Population(50, data, gene);
            RunTraining = true;
        }

        public void RunTrainingScenarios()
        {
            while (RunTraining || RunOnce)
            {
                if (!Pop.Done())
                {
                    //if any players are alive then update them
                    Pop.UpdateAlive();
                }
                else
                {
                    //all dead
                    //genetic algorithm
                    Pop.NaturalSelection();
                    if (Pop.BestSmartMonster != null)
                    {
                        BestGene = Pop.BestSmartMonster.Brain;
                    }
                }
                RunOnce = false;
            }
        }

        public Genome BrainTransplant()
        {
            return BestGene;
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