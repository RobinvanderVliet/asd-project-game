using Creature.Creature.NeuralNetworking;
using Creature.Creature.StateMachine;
using System;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Creature
{
    public class SmartMonster : ICreature
    {
        public ICreatureData creatureData;
        private readonly IDataGatheringService _dataGatheringService;
        public SmartCreatureActions smartactions;
        public TrainingMapGenerator trainingMapGenerator;

        public float fitness;
        public Genome brain;
        public Boolean replay = false;

        public float unadjestedFitness;
        public int bestScore = 0;

        public int score;
        public int gen = 0;

        public static readonly int genomeInputs = 14;
        public static readonly int genomeOutputs = 6;

        public float[] vision = new float[genomeInputs];
        public float[] decision = new float[genomeOutputs];

        //Data for fitnessCalculation
        public int lifeSpan = 0;

        public Boolean dead = false;
        public int DamageDealt { get; set; } = 0;
        public int DamageTaken { get; set; } = 0;
        public int HealthHealed { get; set; } = 0;
        public int StatsGained { get; set; } = 0;
        public int EnemysKilled { get; set; } = 0;

        public float currDistanceToPlayer;
        public float currDistanceToMonster;

        public ICreatureStateMachine CreatureStateMachine => null;

        [ExcludeFromCodeCoverage]
        public SmartMonster(ICreatureData creatureData)
        {
            this.creatureData = creatureData;
            this.trainingMapGenerator = new TrainingMapGenerator();
            this.smartactions = new SmartCreatureActions(trainingMapGenerator.trainingmap);
            this._dataGatheringService = new DataGatheringService();
            brain = new Genome(genomeInputs, genomeOutputs);
        }

        [ExcludeFromCodeCoverage]
        public void ApplyDamage(double amount)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public void HealAmount(double amount)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public void Show()
        {
            //maye use this to sperate the training settings
        }

        public void Update()
        {
            _dataGatheringService.CheckNewPosition(this);
            if (lifeSpan > 1000)
            {
                dead = true;
            }
            lifeSpan++;
            foreach (TrainerAI monster in trainingMapGenerator.monsters)
            {
                monster.update(this);
            }
            foreach (TrainerAI player in trainingMapGenerator.players)
            {
                player.update(this);
            }
        }

        public void Look()
        {
            //get smartMonster x cord
            vision[0] = creatureData.Position.X;
            //get smartMonster y cord
            vision[1] = creatureData.Position.Y;
            //get smartMonster damage
            vision[2] = creatureData.Damage;
            //get smartMonster health
            vision[3] = (float)creatureData.Health;
            //calculate closest player and monster
            _dataGatheringService.ScanMap(this, creatureData.VisionRange);
            // needs to be player location in X and Y
            //get distance to player
            vision[4] = _dataGatheringService.distanceToClosestPlayer;
            //get distance to monster
            vision[5] = _dataGatheringService.distanceToClosestMonster;

            if (_dataGatheringService.closestPlayer == null)
            {
                vision[6] = 0;
                vision[7] = 0;
                vision[8] = 0;
                vision[9] = 0;
            }
            else
            {
                //getplayerhealth
                vision[6] = (float)_dataGatheringService.closestPlayer.health;
                //get player damage
                vision[7] = _dataGatheringService.closestPlayer.damage;
                //get player x location
                vision[8] = _dataGatheringService.closestPlayer.location.X;
                //get player y location
                vision[9] = _dataGatheringService.closestPlayer.location.Y;
            }
            if (_dataGatheringService.closestMonster == null)
            {
                vision[10] = 0;
                vision[11] = 0;
                vision[12] = 0;
                vision[13] = 0;
            }
            else
            {
                //getplayerhealth
                vision[10] = (float)_dataGatheringService.closestMonster.health;
                //get player damage
                vision[11] = _dataGatheringService.closestMonster.damage;
                //get player x location
                vision[12] = _dataGatheringService.closestMonster.location.X;
                //get player y location
                vision[13] = _dataGatheringService.closestMonster.location.Y;
            }
            //get player stamina
            //get monster stamina?
            //get usabel item
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
                //Wander action nneds to be split up in directions
                smartactions.Wander(this, creatureData.Position);

                return;
            }

            switch (maxIndex)
            {
                case 0:
                    //Attack action
                    smartactions.Attack(_dataGatheringService.closestPlayer, this);
                    score = +20;
                    break;

                case 1:
                    //Flee action
                    smartactions.Flee(_dataGatheringService.closestPlayer, this);
                    score = -8;
                    break;

                case 2:
                    //UseItem action
                    score = -10;
                    break;

                case 3:
                    smartactions.RunToMonster(_dataGatheringService.closestMonster, this);
                    score = -3;
                    //Run to Monster action
                    break;

                case 4:
                    //Grab item action
                    score = -10;
                    break;

                case 5:
                    //Move up action
                    smartactions.WalkUp(this);
                    break;

                case 6:
                    //Move down action
                    smartactions.WalkDown(this);
                    break;

                case 7:
                    //Move left action
                    smartactions.WalkLeft(this);
                    break;

                case 8:
                    //Move right action
                    smartactions.WalkRight(this);
                    break;
            }
        }

        //returns a clone of this player with the same brain
        [ExcludeFromCodeCoverage]
        public SmartMonster Clone()
        {
            SmartMonster clone = new SmartMonster(creatureData);
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
            if (EnemysKilled < 0)
            {
                killPoints += 100000000 * EnemysKilled;
            }
            if (dead)
            {
                deathpoints = -100;
            }
            fitness =
                (float)((DamageDealt * 5 - DamageTaken * 2) + (lifeSpan / 300) + HealthHealed + StatsGained + killPoints + deathpoints + score);
        }

        [ExcludeFromCodeCoverage]
        public SmartMonster Crossover(SmartMonster parent2)
        {
            SmartMonster child = new SmartMonster(parent2.creatureData);
            child.brain = brain.Crossover(parent2.brain);
            child.brain.GenerateNetwork();
            return child;
        }

        //since there is some randomness in games sometimes when we want to replay the game we need to remove that randomness
        //this fuction does that
        [ExcludeFromCodeCoverage]
        public SmartMonster CloneForReplay()
        {
            SmartMonster clone = new SmartMonster(creatureData);
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