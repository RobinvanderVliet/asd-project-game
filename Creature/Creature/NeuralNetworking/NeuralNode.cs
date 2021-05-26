using System;
using System.Collections.Generic;

namespace Creature.Creature.NeuralNetworking
{
    public class NeuralNode
    {

        public int number {get; set;}
        public float inputSum = 0;
        public float outputValue = 0;
        public List<ConnectionGene> outputConnections {get; set;}
        public int layer {get; set;}

        public NeuralNode(int number)
        {
            this.number = number;
        }

        //the node sends its output to the inputs of the nodes its connected to
        public void Engage()
        {
            if (layer != 0)
            {
                //no sigmoid for the inputs and bias
                outputValue = Sigmoid(inputSum);
            }

            for (int i = 0; i < outputConnections.Count; i++)
            {
                //for each connection
                if (outputConnections[i].enabled)
                {
                    //add the weighted output to the sum of the inputs of whatever node this node is connected to
                    outputConnections[i].toNode.inputSum += outputConnections[i].weight * outputValue;
                }
            }
        }

        //sigmoid activation function
        public float Sigmoid(float x)
        {
            float k = (float)Math.Exp(x);
            return k / (1.0f + k);
        }

        //returns whether this node connected to the parameter node
        //used when adding a new connection 
        public Boolean IsConnectedTo(NeuralNode node)
        {
            if (node.layer == layer)
            {
                //nodes in the same layer cannot be connected
                return false;
            }

            //you get it
            if (node.layer < layer)
            {
                for (int i = 0; i < node.outputConnections.Count; i++)
                {
                    if (node.outputConnections[i].toNode == this)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < outputConnections.Count; i++)
                {
                    if (outputConnections[i].toNode == node)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //returns a copy of this node
        public NeuralNode Clone()
        {
            NeuralNode clone = new NeuralNode(number);
            clone.layer = layer;
            return clone;
        }

    }
}
