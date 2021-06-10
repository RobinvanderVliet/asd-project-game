using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking
{
    [ExcludeFromCodeCoverage]
    public class ConnectionHistory
    {
        public int FromNode;
        public int ToNode;
        public int InnovationNumber;

        public List<int> InnovationNumbers;

        public ConnectionHistory(int from, int to, int inno, List<int> innovationNos)
        {
            FromNode = from;
            ToNode = to;
            InnovationNumber = inno;
            InnovationNumbers = innovationNos;
        }

        //returns whether the genome matches the original genome and the connection is between the same nodes
        public bool Matches(Genome genome, NeuralNode from, NeuralNode to)
        {
            if (genome.Genes.Count == InnovationNumbers.Count && (from.Number == FromNode && to.Number == ToNode))
            {
                //next check if all the innovation numbers match from the genome
                for (int i = 0; i < genome.Genes.Count; i++)
                {
                    if (!InnovationNumbers.Contains(genome.Genes[i].InnovationNo))
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