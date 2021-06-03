using Creature.Creature.NeuralNetworking;
using System;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration;
using WorldGeneration.StateMachine.Data;

namespace Creature.Creature
{
    [ExcludeFromCodeCoverage]
    public class SmartMonsterForTraining : Monster
    {
        public MonsterData creatureData;
        private readonly DataGatheringServiceForTraining _dataGatheringService;
        public SmartCreatureTrainingActions smartactions;
        public TrainingMapGenerator trainingMapGenerator;

        public float fitness;
        public Genome brain;
        public Boolean replay = false;

        public float unadjestedFitness;
        public int bestScore = 0;

        public int score;
        public int gen = 0;

        public static readonly int genomeInputs = 14;
        public static readonly int genomeOutputs = 7;

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

        public SmartMonsterForTraining(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol, id)
        {
            this.creatureData =
            new MonsterData
                (
                14,
                14,
                0
                );
            this.trainingMapGenerator = new TrainingMapGenerator();
            this.smartactions = new SmartCreatureTrainingActions(trainingMapGenerator.trainingmap);
            this._dataGatheringService = new DataGatheringServiceForTraining();
            brain = new Genome(genomeInputs, genomeOutputs);
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

        public void Update()
        {
            _dataGatheringService.CheckNewPosition(this);
            if (trainingMapGenerator.AllPlayersDead() || lifeSpan >= 1000)
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
                smartactions.Wander(this);
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
                    smartactions.RunToMonster(_dataGatheringService.closestMonster, this);
                    score = -3;
                    //Run to Monster action
                    break;

                case 3:
                    //Move up action
                    smartactions.WalkUp(this);
                    break;

                case 4:
                    //Move down action
                    smartactions.WalkDown(this);
                    break;

                case 5:
                    //Move left action
                    smartactions.WalkLeft(this);
                    break;

                case 6:
                    //Move right action
                    smartactions.WalkRight(this);
                    break;
            }
        }

        //returns a clone of this player with the same brain
        public SmartMonsterForTraining Clone()
        {
            SmartMonsterForTraining clone = new SmartMonsterForTraining("trainee", 14, 14, "D", "hkljubadfilkubh");
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
            if (EnemysKilled > 0)
            {
                killPoints += 1000 * EnemysKilled;
            }
            if (dead)
            {
                deathpoints = -100;
            }
            fitness =
                (float)((DamageDealt * 10 - DamageTaken * 2)/* + (lifeSpan / 300)*/ + HealthHealed + StatsGained + killPoints + deathpoints + score);
            score = (int)fitness;
        }

        public SmartMonsterForTraining Crossover(SmartMonsterForTraining parent2)
        {
            SmartMonsterForTraining child = new SmartMonsterForTraining("trainee", 14, 14, "D", "hkljubadfilkubh");
            child.brain = brain.Crossover(parent2.brain);
            child.brain.GenerateNetwork();
            return child;
        }

        //since there is some randomness in games sometimes when we want to replay the game we need to remove that randomness
        //this fuction does that
        public SmartMonsterForTraining CloneForReplay()
        {
            SmartMonsterForTraining clone = new SmartMonsterForTraining("trainee", 14, 14, "D", "hkljubadfilkubh");
            clone.brain = brain.Clone();
            clone.fitness = fitness;
            clone.brain.GenerateNetwork();
            clone.gen = gen;
            clone.bestScore = score;
            clone.replay = true;
            return clone;
        }
    }
}