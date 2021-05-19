using System;
using System.Collections.Generic;

namespace Creature.Creature.NeuralNetworking
{
    public class Species
    {
        public List<SmartMonster> creatures;
        public float bestFitness = 0;
        public float averageFitness = 0;
        public int staleness = 0;
        public Genome rep;
        public SmartMonster champ;

        public float excessCoeff = 1;
        public float weightDiffCoeff = 0.5f;
        public float compatibilityThreshold = 3;

        Random random = new Random();
        public Species()
        {

        }

        public Species(SmartMonster sm)
        {
            creatures.Add(sm);
            bestFitness = sm.fitness;
            rep = sm.brain.Clone();
        }

        //add a player to the species
        public void AddToSpecies(SmartMonster p)
        {
            creatures.Add(p);
        }

        public Boolean SameSpecies(Genome g)
        {
            float compatibility;
            float excessAndDisjoint = GetExcessDisjoint(g, rep);
            float averageWeightDiff = AverageWeightDiff(g, rep);

            float largeGenomeNormaliser = g.genes.Count - 20;
            if (largeGenomeNormaliser < 1)
            {
                largeGenomeNormaliser = 1;
            }

            compatibility = (excessCoeff * excessAndDisjoint / largeGenomeNormaliser) + (weightDiffCoeff * averageWeightDiff);//compatablilty formula
            return (compatibilityThreshold > compatibility);
        }

        //returns the number of excess and disjoint genes between the 2 input genomes
        //i.e. returns the number of genes which dont match
        public float GetExcessDisjoint(Genome brain1, Genome brain2)
        {
            float matching = 0.0f;
            for (int i = 0; i < brain1.genes.Count; i++)
            {
                for (int j = 0; j < brain2.genes.Count; j++)
                {
                    if (brain1.genes[i].innovationNo == brain2.genes[j].innovationNo)
                    {
                        matching++;
                        break;
                    }
                }
            }
            return (brain1.genes.Count + brain2.genes.Count - 2 * (matching));//return no of excess and disjoint genes
        }

        //returns the avereage weight difference between matching genes in the input genomes
        public float AverageWeightDiff(Genome brain1, Genome brain2)
        {
            if (brain1.genes.Count == 0 || brain2.genes.Count == 0)
            {
                return 0;
            }

            float matching = 0;
            float totalDiff = 0;
            for (int i = 0; i < brain1.genes.Count; i++)
            {
                for (int j = 0; j < brain2.genes.Count; j++)
                {
                    if (brain1.genes[i].innovationNo == brain2.genes[j].innovationNo)
                    {
                        matching++;
                        totalDiff += Math.Abs(brain1.genes[i].weight - brain2.genes[j].weight);
                        break;
                    }
                }
            }
            if (matching == 0)
            {//divide by 0 error
                return 100;
            }
            return totalDiff / matching;
        }

        //sorts the species by fitness 
        public void SortSpecies()
        {

            List<SmartMonster> temp = new List<SmartMonster>();

            //selection short 
            for (int i = 0; i < creatures.Count; i++)
            {
                float max = 0;
                int maxIndex = 0;
                for (int j = 0; j < creatures.Count; j++)
                {
                    if (creatures[j].fitness > max)
                    {
                        max = creatures[j].fitness;
                        maxIndex = j;
                    }
                }
                temp.Add(creatures[maxIndex]);
                creatures.RemoveAt(maxIndex);
                i--;
            }

            creatures = temp;
            if (creatures.Count == 0)
            {
                Console.WriteLine("fucking");
                staleness = 200;
                return;
            }
            //if new best player
            if (creatures[0].fitness > bestFitness)
            {
                staleness = 0;
                bestFitness = creatures[0].fitness;
                rep = creatures[0].brain.Clone();
            }
            else
            {
                //if no new best player
                staleness++;
            }
        }

        //simple stuff
        public void SetAverage()
        {

            float sum = 0;
            for (int i = 0; i < creatures.Count; i++)
            {
                sum += creatures[i].fitness;
            }
            averageFitness = sum / creatures.Count;
        }

        //gets baby from the players in this species
        public SmartMonster GiveMeBaby(List<ConnectionHistory> innovationHistory)
        {
            SmartMonster baby;
            if (random.Next(1) < 0.25)
            {
                //25% of the time there is no crossover and the child is simply a clone of a random(ish) player
                baby = SelectPlayer().Clone();
            }
            else
            {
                //75% of the time do crossover 

                //get 2 random(ish) parents 
                SmartMonster parent1 = SelectPlayer();
                SmartMonster parent2 = SelectPlayer();

                //the crossover function expects the highest fitness parent to be the object and the lowest as the argument
                if (parent1.fitness < parent2.fitness)
                {
                    baby = parent2.Crossover(parent1);
                }
                else
                {
                    baby = parent1.Crossover(parent2);
                }
            }
            baby.brain.Mutate(innovationHistory);//mutate that baby brain
            return baby;
        }

        //selects a player based on it fitness
        public SmartMonster SelectPlayer()
        {
            float fitnessSum = 0;
            for (int i = 0; i < creatures.Count; i++)
            {
                fitnessSum += creatures[i].fitness;
            }

            float rand = NextFloat(random);
            float runningSum = 0;

            for (int i = 0; i < creatures.Count; i++)
            {
                runningSum += creatures[i].fitness;
                if (runningSum > rand)
                {
                    return creatures[i];
                }
            }
            //unreachable code to make the parser happy
            return creatures[0];
        }

        static float NextFloat(Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            // choose -149 instead of -126 to also generate subnormal floats (*)
            double exponent = Math.Pow(2.0, random.Next(-126, 128));
            return (float)(mantissa * exponent);
        }

        //kills off bottom half of the species
        public void Cull()
        {
            if (creatures.Count > 2)
            {
                for (int i = creatures.Count / 2; i < creatures.Count; i++)
                {
                    creatures.RemoveAt(i);
                    i--;
                }
            }
        }
        
        //in order to protect unique players, the fitnesses of each player is divided by the number of players in the species that that player belongs to 
        public void FitnessSharing()
        {
            for (int i = 0; i < creatures.Count; i++)
            {
                creatures[i].fitness /= creatures.Count;
            }
        }
    }
}
