using Creature.Creature.NeuralNetworking;
using Creature.Creature.StateMachine;
using System;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature
{
    public class SmartMonster : ICreature
    {
        private ICreatureData _creatureData;

        public float fitness;
        public Genome brain;
        public Boolean replay = false;

        public float unadjestedFitness;
        public int lifeSpan = 0;
        public int bestScore = 0;

        public Boolean dead;
        public int score;
        public int gen = 0;

        public static int genomeInputs = 11;
        public static int genomeOutputs = 6;

        public float[] vision = new float[genomeInputs];
        public float[] decision = new float[genomeOutputs];

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

        public void Update()
        {
            //Do actions
        }

        public void Look()
        {
            vision[0] = (_creatureData.IsAlive) ? 1 : 0;
            vision[1] = _creatureData.Position.X;
            vision[2] = _creatureData.Position.Y;
            vision[3] = _creatureData.VisionRange;
            vision[4] = _creatureData.Damage;
            vision[5] = (float) _creatureData.Health;

            //getplayerhealth
            //get player stamina
            //get monster stamina?
            //getusabelitem
            //get distance to player
            //get distance to monster
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
                //wander action
                return;
            }

            switch (maxIndex)
            {
                case 0:
                //Attack atction
                    break;
                case 1:
                //Flee action
                    break;
                case 2:
                //UseItem action
                    break;
                case 3:
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
            /*
             * fitness calculation
             * 
             * damageDealt*1 - damageTaken*1 + TimeAlive * 0.1 + healthGained*0.2 + statsGained*0.5 + EnemysKilled*100000000000 - Died*100
             */
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
