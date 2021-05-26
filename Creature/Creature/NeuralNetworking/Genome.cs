using System;
using System.Collections.Generic;

namespace Creature.Creature.NeuralNetworking
{
    public class Genome
    {
        public int nextConnectionNo = 1000;

        public List<ConnectionGene> genes;
        public List<NeuralNode> nodes;

        public int inputs;
        public int outputs;
        public int layers = 2;
        public int nextNode = 0;
        public int biasNode;

        public List<NeuralNode> network;

        Random random = new Random();

        public Genome(int inputs, int outputs)
        {
            this.inputs = inputs;
            this.outputs = outputs;

            //create input nodes
            for (int i = 0; i < inputs; i++)
            {
                nodes.Add(new NeuralNode(i + inputs));
                nextNode++;
                nodes[i].layer = 0;
            }

            //create output nodes
            for (int i = 0; i < outputs; i++)
            {
                nodes.Add(new NeuralNode(i + outputs));
                nodes[i].layer = 1;
                nextNode++;
            }

            //bias node
            nodes.Add(new NeuralNode(nextNode));
            biasNode = nextNode;
            nextNode++;
            nodes[biasNode].layer = 0;
        }

        //return node that matches number
        public NeuralNode GetNode(int nodeNumber)
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
        public void ConnectNodes()
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                nodes[i].outputConnections.Clear();
            }

            for (int i = 0; i < genes.Count; i++)
            {
                genes[i].fromNode.outputConnections.Add(genes[i]);
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

        //sets up the NN as a list of nodes in order to be engaged 

        public void GenerateNetwork()
        {
            ConnectNodes();
            network = new List<NeuralNode>();
            //for each layer add the node in that layer, since layers cannot connect to themselves there is no need to order the nodes within a layer

            for (int l = 0; l < layers; l++)
            {
                //for each layer
                for (int i = 0; i < nodes.Count; i++)
                {
                    //for each node
                    if (nodes[i].layer == l)
                    {
                        //if that node is in that layer
                        network.Add(nodes[i]);
                    }
                }
            }
        }

        //mutate the NN by adding a new node
        //it does this by picking a random connection and disabling it then 2 new connections are added 
        //1 between the input node of the disabled connection and the new node
        //and the other between the new node and the output of the disabled connection
        public void AddNode(List<ConnectionHistory> innovationHistory)
        {
            //pick a random connection to create a node between
            if (genes.Count == 0)
            {
                AddConnection(innovationHistory);
                return;
            }
            int randomConnection = (random.Next(genes.Count));

            while (genes[randomConnection].fromNode == nodes[biasNode] && genes.Count != 1)
            {
                //dont disconnect bias
                randomConnection = (random.Next(genes.Count));
            }

            genes[randomConnection].enabled = false;//disable it

            int newNodeNo = nextNode;
            nodes.Add(new NeuralNode(newNodeNo));
            nextNode++;
            //add a new connection to the new node with a weight of 1
            int connectionInnovationNumber = GetInnovationNumber(innovationHistory, genes[randomConnection].fromNode, GetNode(newNodeNo));
            genes.Add(new ConnectionGene(genes[randomConnection].fromNode, GetNode(newNodeNo), 1, connectionInnovationNumber));


            connectionInnovationNumber = GetInnovationNumber(innovationHistory, GetNode(newNodeNo), genes[randomConnection].toNode);
            //add a new connection from the new node with a weight the same as the disabled connection
            genes.Add(new ConnectionGene(GetNode(newNodeNo), genes[randomConnection].toNode, genes[randomConnection].weight, connectionInnovationNumber));
            GetNode(newNodeNo).layer = genes[randomConnection].fromNode.layer + 1;


            connectionInnovationNumber = GetInnovationNumber(innovationHistory, nodes[biasNode], GetNode(newNodeNo));
            //connect the bias to the new node with a weight of 0 
            genes.Add(new ConnectionGene(nodes[biasNode], GetNode(newNodeNo), 0, connectionInnovationNumber));

            //if the layer of the new node is equal to the layer of the output node of the old connection then a new layer needs to be created
            //more accurately the layer numbers of all layers equal to or greater than this new node need to be incrimented
            if (GetNode(newNodeNo).layer == genes[randomConnection].toNode.layer)
            {
                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    //dont include this newest node
                    if (nodes[i].layer >= GetNode(newNodeNo).layer)
                    {
                        nodes[i].layer++;
                    }
                }
                layers++;
            }
            ConnectNodes();
        }

        //adds a connection between 2 nodes which aren't currently connected
        public void AddConnection(List<ConnectionHistory> innovationHistory)
        {
            //cannot add a connection to a fully connected network
            if (FullyConnected())
            {
                Console.WriteLine("connection failed");
                return;
            }


            //get random nodes
            int randomNode1 = (random.Next(nodes.Count));
            int randomNode2 = (random.Next(nodes.Count));
            while (BadConnectionNodeHasBeenMade(randomNode1, randomNode2))
            {
                //while the random nodes are no good
                //get new ones
                randomNode1 = (random.Next(nodes.Count));
                randomNode2 = (random.Next(nodes.Count));
            }
            int temp;
            if (nodes[randomNode1].layer > nodes[randomNode2].layer)
            {
                //if the first random node is after the second then switch
                temp = randomNode2;
                randomNode2 = randomNode1;
                randomNode1 = temp;
            }

            //get the innovation number of the connection
            //this will be a new number if no identical genome has mutated in the same way 
            int connectionInnovationNumber = GetInnovationNumber(innovationHistory, nodes[randomNode1], nodes[randomNode2]);
            //add the connection with a random array

            genes.Add(new ConnectionGene(nodes[randomNode1], nodes[randomNode2], random.Next(-1, 1), connectionInnovationNumber));//changed this so if error here
            ConnectNodes();
        }

        public Boolean BadConnectionNodeHasBeenMade(int r1, int r2)
        {
            if (nodes[r1].layer == nodes[r2].layer) return true; // if the nodes are in the same layer 
            if (nodes[r1].IsConnectedTo(nodes[r2])) return true; //if the nodes are already connected



            return false;
        }

        //returns the innovation number for the new mutation
        //if this mutation has never been seen before then it will be given a new unique innovation number
        //if this mutation matches a previous mutation then it will be given the same innovation number as the previous one
        public int GetInnovationNumber(List<ConnectionHistory> innovationHistory, NeuralNode from, NeuralNode to)
        {
            Boolean isNew = true;
            int connectionInnovationNumber = nextConnectionNo;
            for (int i = 0; i < innovationHistory.Count; i++)
            {
                //for each previous mutation
                if (innovationHistory[i].Matches(this, from, to))
                {
                    //if match found
                    isNew = false;//its not a new mutation
                    connectionInnovationNumber = innovationHistory[i].innovationNumber; //set the innovation number as the innovation number of the match
                    break;
                }
            }

            if (isNew)
            {
                //if the mutation is new then create an arrayList of integers representing the current state of the genome
                List<int> innoNumbers = new List<int>();
                for (int i = 0; i < genes.Count; i++)
                {
                    //set the innovation numbers
                    innoNumbers.Add(genes[i].innovationNo);
                }

                //then add this mutation to the innovationHistory 
                innovationHistory.Add(new ConnectionHistory(from.number, to.number, connectionInnovationNumber, innoNumbers));
                nextConnectionNo++;
            }
            return connectionInnovationNumber;
        }

        //returns whether the network is fully connected or not
        public Boolean FullyConnected()
        {
            int maxConnections = 0;
            int[] nodesInLayers = new int[layers];//array which stored the amount of nodes in each layer

            //populate array
            for (int i = 0; i < nodes.Count; i++)
            {
                nodesInLayers[nodes[i].layer] += 1;
            }

            //for each layer the maximum amount of connections is the number in this layer * the number of nodes infront of it
            //so lets add the max for each layer together and then we will get the maximum amount of connections in the network
            for (int i = 0; i < layers - 1; i++)
            {
                int nodesInFront = 0;
                for (int j = i + 1; j < layers; j++)
                {
                    //for each layer infront of this layer
                    nodesInFront += nodesInLayers[j];//add up nodes
                }

                maxConnections += nodesInLayers[i] * nodesInFront;
            }

            if (maxConnections == genes.Count)
            {
                //if the number of connections is equal to the max number of connections possible then it is full
                return true;
            }
            return false;
        }

        //mutates the genome
        public void Mutate(List<ConnectionHistory> innovationHistory)
        {
            if (genes.Count == 0)
            {
                AddConnection(innovationHistory);
            }

            float rand1 = random.Next(1);
            if (rand1 < 0.8)
            { 
                // 80% of the time mutate weights
                for (int i = 0; i < genes.Count; i++)
                {
                    genes[i].MutateWeight();
                }
            }
            //5% of the time add a new connection
            float rand2 = random.Next(1);
            if (rand2 < 0.08)
            {
                AddConnection(innovationHistory);
            }


            //1% of the time add a node
            float rand3 = random.Next(1);
            if (rand3 < 0.02)
            {
                AddNode(innovationHistory);
            }
        }

        //called when this Genome is better that the other parent
        public Genome Crossover(Genome parent2)
        {
            Genome child = new Genome(inputs, outputs, true);
            child.genes.Clear();
            child.nodes.Clear();
            child.layers = layers;
            child.nextNode = nextNode;
            child.biasNode = biasNode;
            List<ConnectionGene> childGenes = new List<ConnectionGene>();//list of genes to be inherrited form the parents
            List<Boolean> isEnabled = new List<Boolean>();
            //all inherrited genes
            for (int i = 0; i < genes.Count; i++)
            {
                Boolean setEnabled = true;//is this node in the chlid going to be enabled

                int parent2gene = MatchingGene(parent2, genes[i].innovationNo);
                if (parent2gene != -1)
                {
                    //if the genes match
                    if (!genes[i].enabled || !parent2.genes[parent2gene].enabled)
                    {
                        //if either of the matching genes are disabled

                        if (random.Next(1) < 0.75)
                        {
                            //75% of the time disabel the childs gene
                            setEnabled = false;
                        }
                    }
                    float rand = random.Next(1);
                    if (rand < 0.5)
                    {
                        childGenes.Add(genes[i]);

                        //get gene from this fucker
                    }
                    else
                    {
                        //get gene from parent2
                        childGenes.Add(parent2.genes[parent2gene]);
                    }
                }
                else
                {
                    //disjoint or excess gene
                    childGenes.Add(genes[i]);
                    setEnabled = genes[i].enabled;
                }
                isEnabled.Add(setEnabled);
            }


            //since all excess and disjoint genes are inherrited from the more fit parent (this Genome) the childs structure is no different from this parent | with exception of dormant connections being enabled but this wont effect nodes
            //so all the nodes can be inherrited from this parent
            for (int i = 0; i < nodes.Count; i++)
            {
                child.nodes.Add(nodes[i].Clone());
            }

            //clone all the connections so that they connect the childs new nodes

            for (int i = 0; i < childGenes.Count; i++)
            {
                child.genes.Add(childGenes[i].Clone(child.GetNode(childGenes[i].fromNode.number), child.GetNode(childGenes[i].toNode.number)));
                child.genes[i].enabled = isEnabled[i];
            }

            child.ConnectNodes();
            return child;
        }

        //create an empty genome
        public Genome(int input, int output, Boolean crossover)
        {
            //set input number and output number
            inputs = input;
            outputs = output;
        }

        //returns whether or not there is a gene matching the input innovation number  in the input genome
        public int MatchingGene(Genome parent2, int innovationNumber)
        {
            for (int i = 0; i < parent2.genes.Count; i++)
            {
                if (parent2.genes[i].innovationNo == innovationNumber)
                {
                    return i;
                }
            }
            return -1; //no matching gene found
        }

        //returns a copy of this genome
        public Genome Clone()
        {

            Genome clone = new Genome(inputs, outputs, true);

            for (int i = 0; i < nodes.Count; i++)
            {
                //copy nodes
                clone.nodes.Add(nodes[i].Clone());
            }

            //copy all the connections so that they connect the clone new nodes

            for (int i = 0; i < genes.Count; i++)
            {
                //copy genes
                clone.genes.Add(genes[i].Clone(clone.GetNode(genes[i].fromNode.number), clone.GetNode(genes[i].toNode.number)));
            }

            clone.layers = layers;
            clone.nextNode = nextNode;
            clone.biasNode = biasNode;
            clone.ConnectNodes();

            return clone;
        }

        //prints out info about the genome to the console 
        public void PrintGenome()
        {
            Console.WriteLine("Print genome  layers:", layers);
            Console.WriteLine("bias node: " + biasNode);
            Console.WriteLine("nodes");
            for (int i = 0; i < nodes.Count; i++)
            {
                Console.WriteLine(nodes[i].number + ",");
            }
            Console.WriteLine("Genes");
            for (int i = 0; i < genes.Count; i++)
            {
                //for each connectionGene 
                Console.WriteLine("gene " + genes[i].innovationNo, "From node " + genes[i].fromNode.number, "To node " + genes[i].toNode.number,
                  "is enabled " + genes[i].enabled, "from layer " + genes[i].fromNode.layer, "to layer " + genes[i].toNode.layer, "weight: " + genes[i].weight);
            }

            Console.WriteLine();
        }

    }
}
