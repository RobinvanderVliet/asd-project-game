using MathNet.Numerics.Distributions;
using System;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking
{
    [ExcludeFromCodeCoverage]
    public class ConnectionGene
    {
        public NeuralNode FromNode;
        public NeuralNode ToNode;
        public float Weight;
        public bool Enabled = true;
        public int InnovationNo;

        public ConnectionGene(NeuralNode from, NeuralNode to, float weight, int inno)
        {
            FromNode = from;
            ToNode = to;
            Weight = weight;
            InnovationNo = inno;
        }

        public void MutateWeight()
        {
            var random = new Random();
            float rand2 = (float)random.NextDouble();
            if (rand2 < 0.3)
            {//30% of the time completely change the weight
                Weight = random.Next(-1, 1);
            }
            else
            {//otherwise slightly change it
                Weight += NextGaussian() / 50;
                //keep weight between bounds
                if (Weight > 1)
                {
                    Weight = 1;
                }
                if (Weight < -1)
                {
                    Weight = -1;
                }
            }
        }

        //P (X<=x) = 1/2 erfc((μ - x)/(sqrt(2) σ))
        private static float NextGaussian()
        {
            double mean = 100;
            double stdDev = 10;

            Normal normalDist = new Normal(mean, stdDev);
            float randomGaussianValue = (float)normalDist.Sample();
            return randomGaussianValue;
        }

        //returns a copy of this connectionGene
        public ConnectionGene Clone(NeuralNode from, NeuralNode to)
        {
            ConnectionGene clone = new ConnectionGene(from, to, Weight, InnovationNo);
            clone.Enabled = Enabled;

            return clone;
        }
    }
}