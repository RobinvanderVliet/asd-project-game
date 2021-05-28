using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Creature.NeuralNetworking
{
    [ExcludeFromCodeCoverage]
    public class ConnectionHistory
    {
        public int fromNode;
        public int toNode;
        public int innovationNumber;

        public List<int> innovationNumbers;

        public ConnectionHistory(int from, int to, int inno, List<int> innovationNos)
        {
            fromNode = from;
            toNode = to;
            innovationNumber = inno;
            innovationNumbers = innovationNos;
        }

        //returns whether the genome matches the original genome and the connection is between the same nodes
        public Boolean Matches(Genome genome, NeuralNode from, NeuralNode to)
        {
            if (genome.genes.Count == innovationNumbers.Count && (from.number == fromNode && to.number == toNode))
            {
                //next check if all the innovation numbers match from the genome
                for (int i = 0; i < genome.genes.Count; i++)
                {
                    if (!innovationNumbers.Contains(genome.genes[i].innovationNo))
                    {
                        return false;
                    }
                }
                //if reached this far then the innovationNumbers match the genes innovation numbers and the connection is between the same nodes
                //so it does match
                return true;
            }
            return false;
        }
    }
}