using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario;
using ASD_Game.World.Models.Characters.StateMachine.Data;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario
{
    [ExcludeFromCodeCoverage]
    public class SmartMonsterForTraining : Monster
    {
        public MonsterData CreatureData;
        private readonly DataGatheringServiceForTraining _dataGatheringService;
        public SmartCreatureTrainingActions Smartactions;
        public TrainingMapGenerator TrainingMapGenerator;

        public float Fitness;
        public Genome Brain;
        public bool Replay = false;

        public float UnadjestedFitness;
        public int BestScore = 0;

        public int Score;
        public int Gen = 0;

        public static readonly int GenomeInputs = 14;
        public static readonly int GenomeOutputs = 7;

        public float[] Vision = new float[GenomeInputs];
        public float[] Decision = new float[GenomeOutputs];

        //Data for fitnessCalculation
        public int LifeSpan = 0;

        public bool Dead = false;
        public int DamageDealt { get; set; } = 0;
        public int DamageTaken { get; set; } = 0;
        public int HealthHealed { get; set; } = 0;
        public int StatsGained { get; set; } = 0;
        public int EnemysKilled { get; set; } = 0;

        public float CurrDistanceToPlayer;
        public float CurrDistanceToMonster;

        public SmartMonsterForTraining(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol, id)
        {
            CreatureData =
            new MonsterData
                (
                14,
                14,
                0
                );
            TrainingMapGenerator = new TrainingMapGenerator();
            Smartactions = new SmartCreatureTrainingActions(TrainingMapGenerator.trainingmap);
            _dataGatheringService = new DataGatheringServiceForTraining();
            Brain = new Genome(GenomeInputs, GenomeOutputs);
        }

        public void Show()
        {
            //maybe use this to sperate the training settings
        }

        public void Update()
        {
            _dataGatheringService.CheckNewPosition(this);
            if (TrainingMapGenerator.AllPlayersDead() || LifeSpan >= 1000)
            {
                Dead = true;
            }
            LifeSpan++;
            foreach (TrainerAI monster in TrainingMapGenerator.monsters)
            {
                monster.Update(this);
            }
            foreach (TrainerAI player in TrainingMapGenerator.players)
            {
                player.Update(this);
            }
        }

        public void Look()
        {
            Vision[0] = CreatureData.Position.X;
            Vision[1] = CreatureData.Position.Y;
            Vision[2] = CreatureData.Damage;
            Vision[3] = (float)CreatureData.Health;
            _dataGatheringService.ScanMap(this, CreatureData.VisionRange);
            Vision[4] = _dataGatheringService.DistanceToClosestPlayer;
            Vision[5] = _dataGatheringService.DistanceToClosestMonster;

            if (_dataGatheringService.ClosestPlayer == null)
            {
                Vision[6] = 0;
                Vision[7] = 0;
                Vision[8] = 0;
                Vision[9] = 0;
            }
            else
            {
                Vision[6] = (float)_dataGatheringService.ClosestPlayer.Health;
                Vision[7] = _dataGatheringService.ClosestPlayer.Damage;
                Vision[8] = _dataGatheringService.ClosestPlayer.Location.X;
                Vision[9] = _dataGatheringService.ClosestPlayer.Location.Y;
            }
            if (_dataGatheringService.ClosestMonster == null)
            {
                Vision[10] = 0;
                Vision[11] = 0;
                Vision[12] = 0;
                Vision[13] = 0;
            }
            else
            {
                Vision[10] = (float)_dataGatheringService.ClosestMonster.Health;
                Vision[11] = _dataGatheringService.ClosestMonster.Damage;
                Vision[12] = _dataGatheringService.ClosestMonster.Location.X;
                Vision[13] = _dataGatheringService.ClosestMonster.Location.Y;
            }
        }

        public void Think()
        {
            float max = 0;
            int maxIndex = 0;
            Decision = Brain.FeedForward(Vision);

            for (int i = 0; i < Decision.Length; i++)
            {
                if (Decision[i] > max)
                {
                    max = Decision[i];
                    maxIndex = i;
                }
            }

            if (max < 0.7)
            {
                Smartactions.Wander(this);
                return;
            }

            switch (maxIndex)
            {
                case 0:
                    Smartactions.Attack(_dataGatheringService.ClosestPlayer, this);
                    Score = +20;
                    break;

                case 1:
                    Smartactions.Flee(_dataGatheringService.ClosestPlayer, this);
                    Score = -8;
                    break;

                case 2:
                    Smartactions.RunToMonster(_dataGatheringService.ClosestMonster, this);
                    Score = -3;
                    break;

                case 3:
                    Smartactions.WalkUp(this);
                    break;

                case 4:
                    Smartactions.WalkDown(this);
                    break;

                case 5:
                    Smartactions.WalkLeft(this);
                    break;

                case 6:
                    Smartactions.WalkRight(this);
                    break;
            }
        }

        //returns a clone of this player with the same brain
        public SmartMonsterForTraining Clone()
        {
            SmartMonsterForTraining clone = new SmartMonsterForTraining("trainee", 14, 14, "D", "id");
            clone.Brain = Brain.Clone();
            clone.Fitness = Fitness;
            clone.Brain.GenerateNetwork();
            clone.Gen = Gen;
            clone.BestScore = Score;
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
            if (Dead)
            {
                deathpoints = -100;
            }
            Fitness =
                (float)((DamageDealt * 10 - DamageTaken * 2)/* + (lifeSpan / 300)*/ + HealthHealed + StatsGained + killPoints + deathpoints + Score);
            Score = (int)Fitness;
        }

        public SmartMonsterForTraining Crossover(SmartMonsterForTraining parent2)
        {
            SmartMonsterForTraining child = new SmartMonsterForTraining("trainee", 14, 14, "D", "id");
            child.Brain = Brain.Crossover(parent2.Brain);
            child.Brain.GenerateNetwork();
            return child;
        }

        //since there is some randomness in games sometimes when we want to replay the game we need to remove that randomness
        //this fuction does that
        public SmartMonsterForTraining CloneForReplay()
        {
            SmartMonsterForTraining clone = new SmartMonsterForTraining("trainee", 14, 14, "D", "id");
            clone.Brain = Brain.Clone();
            clone.Fitness = Fitness;
            clone.Brain.GenerateNetwork();
            clone.Gen = Gen;
            clone.BestScore = Score;
            clone.Replay = true;
            return clone;
        }
    }
}