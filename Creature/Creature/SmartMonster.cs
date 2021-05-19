using Creature.Creature.NeuralNetworking;
using Creature.Creature.StateMachine;
using System;

namespace Creature.Creature
{
    public class SmartMonster : ICreature
    {
        public float fitness;
        public Genome brain;
        public Boolean replay = false;

        public float unadjestedFitness;
        public int lifeSpan = 0;
        public int bestScore = 0;

        public Boolean dead;
        public int score;
        public int gen = 0;

        public static int genomeInputs = 5;
        public static int genomeOutputs = 5;

        public float[] vision = new float[genomeInputs];
        public float[] decision = new float[genomeOutputs];

        public ICreatureStateMachine CreatureStateMachine => throw new NotImplementedException();

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

        }

        public void Update()
        {

        }

        public void Look()
        {

        }

        public void Think() 
        {

        }

        //returns a clone of this player with the same brian
        public SmartMonster Clone()
        {
            SmartMonster clone = new SmartMonster();
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
            
        }

        public SmartMonster Crossover(SmartMonster parent2)
        {
            SmartMonster child = new SmartMonster();
            child.brain = brain.Crossover(parent2.brain);
            child.brain.GenerateNetwork();
            return child;
        }

        //since there is some randomness in games sometimes when we want to replay the game we need to remove that randomness
        //this fuction does that

        public SmartMonster CloneForReplay()
        {
            SmartMonster clone = new SmartMonster();
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
