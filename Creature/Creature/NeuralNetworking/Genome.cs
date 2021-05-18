using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Creature.NeuralNetworking
{
    class Genome
    {
        public List<ConnectionGene> genes;
        public List<Node> nodes;

        public int inputs;
        public int outputs;
        public int layers = 2;
        public int nextNode = 0;
        public int biasNode;

        public List<Node> network;

        public Genome(int inputs, int outputs)
        {
            this.inputs = inputs;
            this.outputs = outputs;

            //create input nodes
            for (int i = 0; i < inputs; i++)
            {
                nodes.Add(new Node(i + inputs));
                nextNode++;
                nodes[i].layer = 0;
            }

            //create output nodes
            for (int i = 0; i < outputs; i++)
            {
                nodes.Add(new Node(i + outputs));
                nodes[i].layer = 1;
                nextNode++;
            }

            //bias node
            nodes.Add(new Node(nextNode));
            biasNode = nextNode;
            nextNode++;
            nodes[biasNode].layer = 0;
        }

        //return node that matches number
        public Node GetNode(int nodeNumber)
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                if(nodes[i].number == nodeNumber)
                {
                    return nodes[i];
                }
            }
            return null;
        }

        //Add connection going out of a node so that is can get the next node during the feed formward
        public void ConnectionNodes()
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                nodes[i].outputConnections.Clear;
            }

            for (int i = 0; i < genes.Count; i++)
            {
                genes[i].fromNode.outputConnections.Add(genes(i));
            }
        }

        //Feed input values into the neural network en get the output values back
        public float[] FeedForward(float[] inputValues)
        //set output
        {
            for (int i = 0; i < inputs; i++)
            {
                nodes[i].outputValue = inputValues[i];
            }
            nodes[biasNode].outputValue = 1;//output of bias is 1

            for (int i = 0; i < network.Count; i++)
            {
                //for each node in the network engage it(see node class for what this does)
                network[i].Engage();
            }

            //the outputs are nodes[inputs] to nodes [inputs+outputs-1]
            float[] outs = new float[outputs];
            for (int i = 0; i < outputs; i++)
            {
                outs[i] = nodes[inputs + i].outputValue;
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                //reset all the nodes for the next feed forward
                nodes[i].inputSum = 0;
            }

            return outs;
        }

    }
}
