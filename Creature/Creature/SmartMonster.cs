using Creature.Creature.NeuralNetworking;
using Creature.Creature.StateMachine;
using System;
using Creature.Creature.StateMachine.Data;
using System.Collections.Generic;
using Creature.Creature.NeuralNetworking.TrainingScenario;

namespace Creature.Creature
{
    public class SmartMonster : ICreature
    {
        private ICreatureData _creatureData;
        private IDataGatheringService _dataGatheringService = new DataGatheringService();
        private SmartCreatureActions smartactions;

        public float fitness;
        public Genome brain;
        public Boolean replay = false;

        public float unadjestedFitness;
        public int bestScore = 0;

        public int score;
        public int gen = 0;

        public static int genomeInputs = 8;
        public static int genomeOutputs = 6;

        public float[] vision = new float[genomeInputs];
        public float[] decision = new float[genomeOutputs];

        //Data for fitnessCalculation
        public static int DamageDealt;
        public static int DamageTaken;
        public int lifeSpan = 0;
        public Boolean dead;
        public static int HealthHealed;
        public static int StatsGained;
        public static Boolean EnemyKilled;


        public ICreatureStateMachine CreatureStateMachine => null;

        public SmartMonster(ICreatureData creatureData)
        {
            _creatureData = creatureData;
        }

        public void ApplyDamage(double amount)
        {
            throw new NotImplementedException();
        }

        public void HealAmount(double amount)
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            //maye use this to sperate the training settings
        }

        public void Look()
        {
            //get smartMonster x cord
            vision[0] = _creatureData.Position.X;
            //get smartMonster y cord
            vision[1] = _creatureData.Position.Y;
            //get smartMonster damage
            vision[2] = _creatureData.Damage;
            //get smartMonster health
            vision[3] = (float) _creatureData.Health;

            //calculate closest player and monster
            _dataGatheringService.ScanMap(_creatureData.Position, _creatureData.VisionRange);
            //get distance to player
            vision[4] = _dataGatheringService.distanceToClosestPlayer;
            //get distance to monster
            vision[5] = _dataGatheringService.distanceToClosestMonster;
            //getplayerhealth
            vision[6] = (float)_dataGatheringService.closestPlayer.CreatureStateMachine.CreatureData.Health;
            //get player damage
            vision[7] = _dataGatheringService.closestPlayer.CreatureStateMachine.CreatureData.Damage;
            //get player stamina

            //get monster stamina?
            //getusabelitem
            //get distance to items
            //get total player stats
            //get total monster stats
            //get attack range
        }

        public void Think() 
        {
            float max = 0;
            int maxIndex = 0;
            //get the output of the neural network
            decision = brain.FeedForward(vision);

            for (int i = 0; i < decision.Length; i++)
            {
                if (decision[i] > max)
                {
                    max = decision[i];
                    maxIndex = i;
                }
            }

            if (max < 0.7)
            {
                //Wander action
                smartactions.Wander(_creatureData.Position);
                return;
            }

            switch (maxIndex)
            {
                case 0:
                    //Attack action
                    smartactions.Attack(_dataGatheringService.closestPlayer, this);
                    break;
                case 1:
                    //Flee action
                    smartactions.Flee(_dataGatheringService.closestPlayer, this);
                    break;
                case 2:
                //UseItem action
                    break;
                case 3:
                    smartactions.RunToMonster(_dataGatheringService.closestMonster, this);
                //Run to Monster action
                    break;
                case 4:
                //Grab item action
                    break;
            }

        }

        //returns a clone of this player with the same brian
        public SmartMonster Clone()
        {
            SmartMonster clone = new SmartMonster(_creatureData);
            clone.brain = brain.Clone();
            clone.fitness = fitness;
            clone.brain.GenerateNetwork();
            clone.gen = gen;
            clone.bestScore = score;
            return clone;
        }

        //for Genetic algorithm
        public void CalculateFitness()
        {
            int killPoints = 0;
            int deathpoints = 0;
            //Fitness calculation
            if(EnemyKilled)
            {
                killPoints =+ 100000000;
            }
            if(Dead)
            {
                deathpoints = -100;
            }
            fitness =
                (float)((DamageDealt - DamageTaken) + (LifeSpan * 0.2) + HealthHealed + StatsGained + killPoints + deathpoints);
        }

        public SmartMonster Crossover(SmartMonster parent2)
        {
            SmartMonster child = new SmartMonster(parent2._creatureData);
            child.brain = brain.Crossover(parent2.brain);
            child.brain.GenerateNetwork();
            return child;
        }

        //since there is some randomness in games sometimes when we want to replay the game we need to remove that randomness
        //this fuction does that

        public SmartMonster CloneForReplay()
        {
            SmartMonster clone = new SmartMonster(_creatureData);
            clone.brain = brain.Clone();
            clone.fitness = fitness;
            clone.brain.GenerateNetwork();
            clone.gen = gen;
            clone.bestScore = score;
            clone.replay = true;
            if (replay)
            {
                
            }
            else
            {
                
            }

            return clone;
        }


    }
}
