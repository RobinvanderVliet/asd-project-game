using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Creature.NeuralNetworking
{
    class Species
    {
        public List<ICreature> creatures;
        public float bestFitness = 0;
        public ICreature champ;
        public float averageFitness = 0;
        int staleness = 0;
        public Genome rep;

        public float excessCoeff = 1;
        public float weightDiffCoeff = 0.5f;
        public float compatibilityThreshold = 3;

        public Species()
        {

        }

        public Species(ICreature c)
        {
            creatures.Add(c);
            bestFitness = c.fitness;
            rep = c.brain.clone();
            champ = c.cloneForReplay();
        }

        public Boolean SameSpecies(Genome g)
        {
            float compatibility;
            float excessAndDisjoint = getExcessDisJoint(g, rep);
            float averageWeightDiff = averageWeightDiff(g, rep);

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
        float getExcessDisjoint(Genome brain1, Genome brain2)
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

    }
}
