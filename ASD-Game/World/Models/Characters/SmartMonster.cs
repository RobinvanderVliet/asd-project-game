using Creature.Creature.NeuralNetworking;
using System;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration;
using WorldGeneration.StateMachine.Data;
using System.Numerics;

namespace Creature.Creature
{
    [ExcludeFromCodeCoverage]
    public class SmartMonster : Monster
    {
        public MonsterData creatureData;
        private DataGatheringService _dataGatheringService;
        public SmartCreatureActions smartactions;

        public Vector2 NextAction { get; set; }

        public Genome brain;
        public Boolean replay = false;

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

        public SmartMonster(string name, int xPosition, int yPosition, string symbol, string id, DataGatheringService datagatheringservice) : base(name, xPosition, yPosition, symbol, id)
        {
            creatureData = CreateMonsterData(0);
            this._dataGatheringService = datagatheringservice;
            this.smartactions = new SmartCreatureActions(this, datagatheringservice);
        }

        public void Update()
        {
            _dataGatheringService.CheckNewPosition(this);
            if (!dead)
            {
                lifeSpan++;
                Look();
                Think();
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
                vision[6] = (float)_dataGatheringService.closestPlayer.Health;
                //get player damage
                vision[7] = 10;//TODO _dataGatheringService.closestPlayer.Damage;
                //get player x location
                vision[8] = _dataGatheringService.closestPlayer.XPosition;
                //get player y location
                vision[9] = _dataGatheringService.closestPlayer.YPosition;
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
                //TODO
                //getplayerhealth
                vision[10] = (float)_dataGatheringService.closestMonster.Health;
                //get player damage
                vision[11] = 10;//TODO _dataGatheringService.closestMonster.Damage;
                //get player x location
                vision[12] = _dataGatheringService.closestMonster.XPosition;
                //get player y location
                vision[13] = _dataGatheringService.closestMonster.YPosition;
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
                    break;

                case 1:
                    //Flee action
                    smartactions.Flee(_dataGatheringService.closestPlayer, this);
                    break;

                case 2:
                    smartactions.RunToMonster(_dataGatheringService.closestMonster, this);
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

        public void SetDifficulty(int difficulty)
        {
            _monsterData = new MonsterData(
                XPosition,
                YPosition,
                difficulty);
        }

        private MonsterData CreateMonsterData(int difficulty)
        {
            return new MonsterData(
                XPosition,
                YPosition,
                difficulty);
        }
    }
}