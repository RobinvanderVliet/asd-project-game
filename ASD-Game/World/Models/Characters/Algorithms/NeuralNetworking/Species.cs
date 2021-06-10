using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking
{
    public class Species
    {
        public List<SmartMonsterForTraining> Creatures = new List<SmartMonsterForTraining>();
        public float BestFitness = 0;
        public float AverageFitness = 0;
        public int Staleness = 0;
        public Genome Rep;
        public SmartMonsterForTraining Champ;

        public float ExcessCoeff = 1;
        public float WeightDiffCoeff = 0.5f;
        public float CompatibilityThreshold = 3;

        public readonly Random Rando = new Random();

        [ExcludeFromCodeCoverage]
        public Species()
        {
        }

        [ExcludeFromCodeCoverage]
        public Species(SmartMonsterForTraining sm)
        {
            Creatures.Add(sm);
            BestFitness = sm.Fitness;
            Rep = sm.Brain.Clone();
            Champ = sm.CloneForReplay();
        }

        //add a player to the species
        public void AddToSpecies(SmartMonsterForTraining p)
        {
            Creatures.Add(p);
        }

        public bool SameSpecies(Genome g)
        {
            float compatibility;
            float excessAndDisjoint = GetExcessDisjoint(g, Rep);
            float averageWeightDiff = AverageWeightDiff(g, Rep);

            float largeGenomeNormaliser = g.Genes.Count - 20;
            if (largeGenomeNormaliser < 1)
            {
                largeGenomeNormaliser = 1;
            }

            compatibility = (ExcessCoeff * excessAndDisjoint / largeGenomeNormaliser) + (WeightDiffCoeff * averageWeightDiff);//compatibility formula
            return (CompatibilityThreshold > compatibility);
        }

        //returns the number of excess and disjoint genes between the 2 input genomes
        //i.e. returns the number of genes which dont match
        public static float GetExcessDisjoint(Genome brain1, Genome brain2)
        {
            float matching = 0.0f;
            for (int i = 0; i < brain1.Genes.Count; i++)
            {
                for (int j = 0; j < brain2.Genes.Count; j++)
                {
                    if (brain1.Genes[i].InnovationNo == brain2.Genes[j].InnovationNo)
                    {
                        matching++;
                        break;
                    }
                }
            }
            return (brain1.Genes.Count + brain2.Genes.Count - 2 * (matching));//return no of excess and disjoint genes
        }

        //returns the average weight difference between matching genes in the input genomes
        public static float AverageWeightDiff(Genome brain1, Genome brain2)
        {
            if (brain1.Genes.Count == 0 || brain2.Genes.Count == 0)
            {
                return 0;
            }

            float matching = 0;
            float totalDiff = 0;
            for (int i = 0; i < brain1.Genes.Count; i++)
            {
                for (int j = 0; j < brain2.Genes.Count; j++)
                {
                    if (brain1.Genes[i].InnovationNo == brain2.Genes[j].InnovationNo)
                    {
                        matching++;
                        totalDiff += Math.Abs(brain1.Genes[i].Weight - brain2.Genes[j].Weight);
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
            for (int i = 0; i < Creatures.Count; i++)
            {
                float max = 0;
                int maxIndex = 0;
                for (int j = 0; j < Creatures.Count; j++)
                {
                    if (Creatures[j].Fitness > max)
                    {
                        max = Creatures[j].Fitness;
                        maxIndex = j;
                    }
                }
                temp.Add(Creatures[maxIndex]);
                Creatures.RemoveAt(maxIndex);
                i--;
            }

            Creatures = temp;
            if (Creatures.Count == 0)
            {
                Staleness = 200;
                return;
            }
            //if new best player
            if (Creatures[0].Fitness > BestFitness)
            {
                Staleness = 0;
                BestFitness = Creatures[0].Fitness;
                Rep = Creatures[0].Brain.Clone();
                Champ = Creatures[0].CloneForReplay();
            }
            else
            {
                //if no new best player
                Staleness++;
            }
        }

        //simple stuff
        public void SetAverage()
        {
            float sum = 0;
            for (int i = 0; i < Creatures.Count; i++)
            {
                sum += Creatures[i].Fitness;
            }
            AverageFitness = sum / Creatures.Count;
        }

        //gets baby from the players in this species
        [ExcludeFromCodeCoverage]
        public SmartMonsterForTraining GiveMeBaby(List<ConnectionHistory> innovationHistory)
        {
            SmartMonsterForTraining baby;
            if ((float)Rando.NextDouble() < 0.25)
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
                if (parent1.Fitness < parent2.Fitness)
                {
                    baby = parent2.Crossover(parent1);
                }
                else
                {
                    baby = parent1.Crossover(parent2);
                }
            }
            baby.Brain.Mutate(innovationHistory);//mutate that baby brain
            return baby;
        }

        //selects a player based on it fitness
        public SmartMonsterForTraining SelectPlayer()
        {
            float fitnessSum = 0;
            for (int i = 0; i < Creatures.Count; i++)
            {
                fitnessSum += Creatures[i].Fitness;
            }

            float rand = (float)Rando.NextDouble();
            float runningSum = 0;

            for (int i = 0; i < Creatures.Count; i++)
            {
                runningSum += Creatures[i].Fitness;
                if (runningSum > rand)
                {
                    return Creatures[i];
                }
            }
            //unreachable code to make the parser happy
            return Creatures[0];
        }

        //kills off bottom half of the species
        public void Cull()
        {
            if (Creatures.Count > 2)
            {
                for (int i = Creatures.Count / 2; i < Creatures.Count; i++)
                {
                    Creatures.RemoveAt(i);
                    i--;
                }
            }
        }

        //in order to protect unique players, the fitnesses of each player is divided by the number of players in the species that that player belongs to
        public void FitnessSharing()
        {
            for (int i = 0; i < Creatures.Count; i++)
            {
                Creatures[i].Fitness /= Creatures.Count;
            }
        }
    }
}