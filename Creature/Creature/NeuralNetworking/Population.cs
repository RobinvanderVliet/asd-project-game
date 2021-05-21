using System;
using System.Collections.Generic;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.NeuralNetworking
{
    public class Population
    {
        public List<SmartMonster> pop = new List<SmartMonster>();
        public SmartMonster bestSmartMonster;//the best ever SmartMonster 
        public int bestScore = 0;//the score of the best ever SmartMonster
        public int gen;
        public List<ConnectionHistory> innovationHistory = new List<ConnectionHistory>();
        public List<SmartMonster> genSmartMonsters = new List<SmartMonster>();
        public List<Species> species = new List<Species>();

        public Boolean massExtinctionEvent = false;
        public Boolean newStage = false;
        public int populationLife = 0;

        public Boolean showNothing = false;

        //constructor
        public Population(int size, ICreatureData creatureData)
        {

            for (int i = 0; i < size; i++)
            {
                pop.Add(new SmartMonster(creatureData));
                pop[i].brain.GenerateNetwork();
                pop[i].brain.Mutate(innovationHistory);
            }
        }

        //update all the SmartMonsters which are alive
        public void UpdateAlive()
        {
            populationLife++;
            for (int i = 0; i < pop.Count; i++)
            {
                if (!pop[i].dead)
                {
                    pop[i].Look();//get inputs for brain 
                    pop[i].Think();//use outputs from neural network
                    pop[i].Update();//move the SmartMonster according to the outputs from the neural network
                    if (!showNothing)
                    {
                        pop[i].Show();
                    }
                }
            }
        }

        //returns true if all the SmartMonsters are dead      sad
        public Boolean Done()
        {
            for (int i = 0; i < pop.Count; i++)
            {
                if (!pop[i].dead)
                {
                    return false;
                }
            }
            return true;
        }

        //sets the best SmartMonster globally and for this gen
        public void SetBestSmartMonster()
        {
            SmartMonster tempBest = species[0].creatures[0];
            tempBest.gen = gen;


            //if best this gen is better than the global best score then set the global best as the best this gen

            if (tempBest.score > bestScore)
            {
                genSmartMonsters.Add(tempBest.CloneForReplay());
                Console.WriteLine("old best:", bestScore);
                Console.WriteLine("new best:", tempBest.score);
                bestScore = tempBest.score;
                bestSmartMonster = tempBest.CloneForReplay();
            }
        }

        //this function is called when all the SmartMonsters in the population are dead and a new generation needs to be made
        public void NaturalSelection()
        {
            Speciate();//seperate the population into species 
            CalculateFitness();//calculate the fitness of each SmartMonster
            SortSpecies();//sort the species to be ranked in fitness order, best first
            if (massExtinctionEvent)
            {
                MassExtinction();
                massExtinctionEvent = false;
            }
            CullSpecies();//kill off the bottom half of each species
            SetBestSmartMonster();//save the best SmartMonster of this gen
            KillStaleSpecies();//remove species which haven't improved in the last 15(ish) generations
            KillBadSpecies();//kill species which are so bad that they cant reproduce


            Console.WriteLine("generation", gen, "Number of mutations", innovationHistory.Count, "species: " + species.Count, "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");


            float averageSum = GetAvgFitnessSum();
            List<SmartMonster> children = new List<SmartMonster>();//the next generation
            Console.WriteLine("Species:");
            for (int j = 0; j < species.Count; j++)
            {//for each species

                Console.WriteLine("best unadjusted fitness:", species[j].bestFitness);
                for (int i = 0; i < species[j].creatures.Count; i++)
                {
                    Console.WriteLine("SmartMonster " + i, "fitness: " + species[j].creatures[i].fitness, "score " + species[j].creatures[i].score, ' ');
                }
                Console.WriteLine();
                children.Add(species[j].champ.Clone());//add champion without any mutation

                int NoOfChildren = (int)((species[j].averageFitness / averageSum * pop.Count) - 1);//the number of children this species is allowed, note -1 is because the champ is already added
                for (int i = 0; i < NoOfChildren; i++)
                {//get the calculated amount of children from this species
                    children.Add(species[j].GiveMeBaby(innovationHistory));
                }
            }

            while (children.Count < pop.Count)
            {//if not enough babies (due to flooring the number of children to get a whole int) 
                children.Add(species[0].GiveMeBaby(innovationHistory));//get babies from the best species
            }
            pop.Clear();
            pop = children; //set the children as the current population
            gen += 1;
            for (int i = 0; i < pop.Count; i++)
            {//generate networks for each of the children
                pop[i].brain.GenerateNetwork();
            }

            populationLife = 0;
        }

        //seperate population into species based on how similar they are to the leaders of each species in the previous gen
        public void Speciate()
        {
            foreach(Species s in species)
            {//empty species
                s.creatures.Clear();
            }
            for (int i = 0; i < pop.Count; i++)
            {//for each SmartMonster
                Boolean speciesFound = false;
                foreach(Species s in species)
                {//for each species
                    if (s.SameSpecies(pop[i].brain))
                    {//if the SmartMonster is similar enough to be considered in the same species
                        s.AddToSpecies(pop[i]);//add it to the species
                        speciesFound = true;
                        break;
                    }
                }
                if (!speciesFound)
                {//if no species was similar enough then add a new species with this as its champion
                    species.Add(new Species(pop[i]));
                }
            }
        }

        //calculates the fitness of all of the SmartMonsters 
        public void CalculateFitness()
        {
            for (int i = 1; i < pop.Count; i++)
            {
                pop[i].CalculateFitness();
            }
        }

        //sorts the SmartMonsters within a species and the species by their fitnesses
        public void SortSpecies()
        {
            //sort the SmartMonsters within a species
            foreach(Species s in species)
            {
                s.SortSpecies();
            }

            //sort the species by the fitness of its best SmartMonster
            //using selection sort like a loser
            List<Species> temp = new List<Species>();
            for (int i = 0; i < species.Count; i++)
            {
                float max = 0;
                int maxIndex = 0;
                for (int j = 0; j < species.Count; j++)
                {
                    if (species[j].bestFitness > max)
                    {
                        max = species[j].bestFitness;
                        maxIndex = j;
                    }
                }
                temp.Add(species[maxIndex]);
                species.RemoveAt(maxIndex);
                i--;
            }
            species = temp;
        }

        //kills all species which haven't improved in 15 generations
        public void KillStaleSpecies()
        {
            for (int i = 2; i < species.Count; i++)
            {
                if (species[i].staleness >= 15)
                {
                    species.RemoveAt(i);
                    i--;
                }
            }
        }

        //if a species sucks so much that it wont even be allocated 1 child for the next generation then kill it now
        public void KillBadSpecies()
        {
            float averageSum = GetAvgFitnessSum();

            for (int i = 1; i < species.Count; i++)
            {
                if (species[i].averageFitness / averageSum * pop.Count < 1)
                {//if wont be given a single child 
                    species.RemoveAt(i);//sad
                    i--;
                }
            }
        }

        //returns the sum of each species average fitness
        public float GetAvgFitnessSum()
        {
            float averageSum = 0;
            foreach(Species s in species)
            {
                averageSum += s.averageFitness;
            }
            return averageSum;
        }

        //kill the bottom half of each species
        public void CullSpecies()
        {
            foreach(Species s in species)
            {
                s.Cull(); //kill bottom half
                s.FitnessSharing();//also while we're at it lets do fitness sharing
                s.SetAverage();//reset averages because they will have changed
            }
        }


        public void MassExtinction()
        {
            for (int i = 5; i < species.Count; i++)
            {
                species.RemoveAt(i);//sad
                i--;
            }
        }
    }
}
