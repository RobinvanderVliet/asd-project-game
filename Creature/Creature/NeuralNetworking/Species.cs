using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Creature.NeuralNetworking
{
    public class Species
    {
        public List<SmartMonsterForTraining> creatures = new List<SmartMonsterForTraining>();
        public float bestFitness = 0;
        public float averageFitness = 0;
        public int staleness = 0;
        public Genome rep;
        public SmartMonsterForTraining champ;

        public float excessCoeff = 1;
        public float weightDiffCoeff = 0.5f;
        public float compatibilityThreshold = 3;

        public readonly Random random = new Random();

        [ExcludeFromCodeCoverage]
        public Species()
        {
        }

        [ExcludeFromCodeCoverage]
        public Species(SmartMonsterForTraining sm)
        {
            creatures.Add(sm);
            bestFitness = sm.fitness;
            rep = sm.brain.Clone();
            champ = sm.CloneForReplay();
        }

        //add a player to the species
        public void AddToSpecies(SmartMonsterForTraining p)
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
        public static float GetExcessDisjoint(Genome brain1, Genome brain2)
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
        public static float AverageWeightDiff(Genome brain1, Genome brain2)
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
            List<SmartMonsterForTraining> temp = new List<SmartMonsterForTraining>();

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
                staleness = 200;
                return;
            }
            //if new best player
            if (creatures[0].fitness > bestFitness)
            {
                staleness = 0;
                bestFitness = creatures[0].fitness;
                rep = creatures[0].brain.Clone();
                champ = creatures[0].CloneForReplay();
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
        [ExcludeFromCodeCoverage]
        public SmartMonsterForTraining GiveMeBaby(List<ConnectionHistory> innovationHistory)
        {
            SmartMonsterForTraining baby;
            if ((float)random.NextDouble() < 0.25)
            {
                //25% of the time there is no crossover and the child is simply a clone of a random(ish) player
                baby = SelectPlayer().Clone();
            }
            else
            {
                //75% of the time do crossover

                //get 2 random(ish) parents
                SmartMonsterForTraining parent1 = SelectPlayer();
                SmartMonsterForTraining parent2 = SelectPlayer();

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
        public SmartMonsterForTraining SelectPlayer()
        {
            float fitnessSum = 0;
            for (int i = 0; i < creatures.Count; i++)
            {
                fitnessSum += creatures[i].fitness;
            }

            float rand = (float)random.NextDouble();
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