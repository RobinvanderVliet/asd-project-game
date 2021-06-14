using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario;
using ASD_Game.World.Models.Characters.StateMachine.Data;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking
{
    public class Population
    {
        public List<SmartMonsterForTraining> Pop = new List<SmartMonsterForTraining>();
        public SmartMonsterForTraining BestSmartMonster;//the best ever SmartMonster
        public int BestScore = 0;//the score of the best ever SmartMonster
        public int Gen;
        public List<ConnectionHistory> InnovationHistory = new List<ConnectionHistory>();
        public List<SmartMonsterForTraining> GenSmartMonsters = new List<SmartMonsterForTraining>();
        public List<Species> Species = new List<Species>();

        public bool MassExtinctionEvent = false;
        public bool NewStage = false;
        public readonly bool ShowNothing = false;
        public int PopulationLife = 0;

        //constructor
        public Population(int size, MonsterData creatureData)
        {
            for (int i = 0; i < size; i++)
            {
                Pop.Add(new SmartMonsterForTraining("trainee", 14, 14, "D", "id"));
                Pop[i].Brain.GenerateNetwork();
                Pop[i].Brain.Mutate(InnovationHistory);
            }
        }

        //constructor for new host
        public Population(int size, MonsterData creatureData, Genome gene)
        {
            for (int i = 0; i < size; i++)
            {
                Pop.Add(new SmartMonsterForTraining("trainee", 14, 14, "D", "id"));
                Pop[i].Brain = gene;
                Pop[i].Brain.Mutate(InnovationHistory);
            }
        }

        //update all the SmartMonsters which are alive
        [ExcludeFromCodeCoverage]
        public void UpdateAlive()
        {
            PopulationLife++;
            for (int i = 0; i < Pop.Count; i++)
            {
                if (!Pop[i].Dead)
                {
                    Pop[i].Look();//get inputs for brain
                    Pop[i].Think();//use outputs from neural network
                    Pop[i].Update();//move the SmartMonster according to the outputs from the neural network
                    if (!ShowNothing)
                    {
                        Pop[i].Show();
                    }
                }
            }
        }

        //returns true if all the SmartMonsters are dead
        public bool Done()
        {
            for (int i = 0; i < Pop.Count; i++)
            {
                if (!Pop[i].Dead)
                {
                    return false;
                }
            }
            return true;
        }

        //sets the best SmartMonster globally and for this gen
        private void SetBestSmartMonster()
        {
            SmartMonsterForTraining tempBest = Species[0].Creatures[0];
            tempBest.Gen = Gen;

            //if best this gen is better than the global best score then set the global best as the best this gen

            if (tempBest.Score > BestScore)
            {
                GenSmartMonsters.Add(tempBest.CloneForReplay());
                BestScore = tempBest.Score;
                BestSmartMonster = tempBest.CloneForReplay(); ;
            }
        }

        //this function is called when all the SmartMonsters in the population are dead and a new generation needs to be made
        public void NaturalSelection()
        {
            Speciate();//seperate the population into species
            CalculateFitness();//calculate the fitness of each SmartMonster
            SortSpecies();//sort the species to be ranked in fitness order, best first
            if (MassExtinctionEvent)
            {
                MassExtinction();
                MassExtinctionEvent = false;
            }
            CullSpecies();//kill off the bottom half of each species
            SetBestSmartMonster();//save the best SmartMonster of this gen
            KillStaleSpecies();//remove species which haven't improved in the last 15(ish) generations
            KillBadSpecies();//kill species which are so bad that they cant reproduce

            float averageSum = GetAvgFitnessSum();
            List<SmartMonsterForTraining> children = new List<SmartMonsterForTraining>();//the next generation
            for (int j = 0; j < Species.Count; j++)
            {
                //for each species
                children.Add(Species[j].Champ.Clone());//add champion without any mutation

                int NoOfChildren = (int)((Species[j].AverageFitness / averageSum * Pop.Count) - 1);//the number of children this species is allowed, note -1 is because the champ is already added
                for (int i = 0; i < NoOfChildren; i++)
                {//get the calculated amount of children from this species
                    children.Add(Species[j].GiveMeBaby(InnovationHistory));
                }
            }

            while (children.Count < Pop.Count)
            {//if not enough babies (due to flooring the number of children to get a whole int)
                children.Add(Species[0].GiveMeBaby(InnovationHistory));//get babies from the best species
            }
            Pop.Clear();
            Pop = children; //set the children as the current population
            foreach (SmartMonsterForTraining child in Pop)
            {
                child.CreatureData = RestoreMonster();
            }
            Gen += 1;
            for (int i = 0; i < Pop.Count; i++)
            {
                //generate networks for each of the children
                Pop[i].Brain.GenerateNetwork();
            }

            PopulationLife = 0;
        }

        //seperate population into species based on how similar they are to the leaders of each species in the previous gen
        private void Speciate()
        {
            foreach (Species s in Species)
            {//empty species
                s.Creatures.Clear();
            }
            for (int i = 0; i < Pop.Count; i++)
            {//for each SmartMonster
                Boolean speciesFound = false;
                foreach (Species s in Species)
                {//for each species
                    if (s.SameSpecies(Pop[i].Brain))
                    {//if the SmartMonster is similar enough to be considered in the same species
                        s.AddToSpecies(Pop[i]);//add it to the species
                        speciesFound = true;
                        break;
                    }
                }
                if (!speciesFound)
                {//if no species was similar enough then add a new species with this as its champion
                    Species.Add(new Species(Pop[i]));
                }
            }
        }

        //calculates the fitness of all of the SmartMonsters
        private void CalculateFitness()
        {
            for (int i = 1; i < Pop.Count; i++)
            {
                Pop[i].CalculateFitness();
            }
        }

        //sorts the SmartMonsters within a species and the species by their fitnesses
        private void SortSpecies()
        {
            //sort the SmartMonsters within a species
            foreach (Species s in Species)
            {
                s.SortSpecies();
            }

            //sort the species by the fitness of its best SmartMonster
            //using selection sort like a bad programmer :)
            List<Species> temp = new List<Species>();
            for (int i = 0; i < Species.Count; i++)
            {
                float max = 0;
                int maxIndex = 0;
                for (int j = 0; j < Species.Count; j++)
                {
                    if (Species[j].BestFitness > max)
                    {
                        max = Species[j].BestFitness;
                        maxIndex = j;
                    }
                }
                temp.Add(Species[maxIndex]);
                Species.RemoveAt(maxIndex);
                i--;
            }
            Species = temp;
        }

        //kills all species which haven't improved in 15 generations
        private void KillStaleSpecies()
        {
            for (int i = 2; i < Species.Count; i++)
            {
                if (Species[i].Staleness >= 15)
                {
                    Species.RemoveAt(i);
                    i--;
                }
            }
        }

        //if a species sucks so much that it wont even be allocated 1 child for the next generation then kill it now
        private void KillBadSpecies()
        {
            float averageSum = GetAvgFitnessSum();

            for (int i = 1; i < Species.Count; i++)
            {
                if (Species[i].AverageFitness / averageSum * Pop.Count < 1)
                {//if wont be given a single child
                    Species.RemoveAt(i);//sad
                    i--;
                }
            }
        }

        //returns the sum of each species average fitness
        private float GetAvgFitnessSum()
        {
            float averageSum = 0;
            foreach (Species s in Species)
            {
                averageSum += s.AverageFitness;
            }
            return averageSum;
        }

        //kill the bottom half of each species
        private void CullSpecies()
        {
            foreach (Species s in Species)
            {
                s.Cull(); //kill bottom half
                s.FitnessSharing();//also while we're at it lets do fitness sharing
                s.SetAverage();//reset averages because they will have changed
            }
        }

        private void MassExtinction()
        {
            for (int i = 5; i < Species.Count; i++)
            {
                Species.RemoveAt(i);//sad
                i--;
            }
        }

        private MonsterData RestoreMonster()
        {
            return new MonsterData
            (
                14,
                14,
                0
            );
        }
    }
}