using Creature.Creature.StateMachine.Data;
using System;
using System.Numerics;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class TrainingScenario
    {
        public Population pop;
        SmartMonster genSmartMonsterTemp;
        private int timeElapsed = 0;

        static void Main(string[] args)
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
                new Vector2(14,14),
                20,
                5,
                200,
                null,
                false
            );
            pop = new Population(50,data);
        }

        public void RunTestScenarios()
        {
            while (true) 
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
                    timeElapsed++;
                }
        }
    }

}
