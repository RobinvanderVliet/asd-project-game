using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking
{
    public class NeuralNode
    {
        public int Number { get; set; }
        public float InputSum = 0;
        public float OutputValue = 0;
        public List<ConnectionGene> OutputConnections { get; set; } = new List<ConnectionGene>();
        public int Layer { get; set; }

        public NeuralNode(int number)
        {
            Number = number;
        }

        //the node sends its output to the inputs of the nodes its connected to
        public void Engage()
        {
            if (Layer != 0)
            {
                //no sigmoid for the inputs and bias
                OutputValue = Sigmoid(InputSum);
            }

            for (int i = 0; i < OutputConnections.Count; i++)
            {
                //for each connection
                if (OutputConnections[i].Enabled)
                {
                    //add the weighted output to the sum of the inputs of whatever node this node is connected to
                    OutputConnections[i].ToNode.InputSum += OutputConnections[i].Weight * OutputValue;
                }
            }
        }

        //sigmoid activation function
        [ExcludeFromCodeCoverage]
        private static float Sigmoid(float x)
        {
            float k = (float)Math.Exp(x);
            return k / (1.0f + k);
        }

        //returns whether this node connected to the parameter node
        //used when adding a new connection
        public bool IsConnectedTo(NeuralNode node)
        {
            if (node.Layer == Layer)
            {
                //nodes in the same layer cannot be connected
                return false;
            }

            //Als een node op een hogere laag zit dan de huidige node kan deze verbonden worden
            //anders niet aangezien je geen connectie kan aanmaken met nodes op dezelfde laag.
            if (node.Layer < Layer)
            {
                for (int i = 0; i < node.OutputConnections.Count; i++)
                {
                    if (node.OutputConnections[i].ToNode == this)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < OutputConnections.Count; i++)
                {
                    if (OutputConnections[i].ToNode == node)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //returns a copy of this node
        [ExcludeFromCodeCoverage]
        public NeuralNode Clone()
        {
            NeuralNode clone = new NeuralNode(Number);
            clone.Layer = Layer;
            return clone;
        }
    }
}