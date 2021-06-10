using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking
{
    public class Genome
    {
        public List<ConnectionGene> Genes = new List<ConnectionGene>();
        public List<NeuralNode> Nodes = new List<NeuralNode>();

        public int Inputs;
        public int Outputs;
        public int Layers = 2;
        public int NextNode = 0;
        public int BiasNode;

        public int NextConnectionNo = 1000;

        public List<NeuralNode> Network = new List<NeuralNode>();

        private readonly Random _random = new Random();

        public Genome(int inputs, int outputs)
        {
            Inputs = inputs;
            Outputs = outputs;

            //create input nodes
            for (int i = 0; i < inputs; i++)
            {
                Nodes.Add(new NeuralNode(i));
                NextNode++;
                Nodes[i].Layer = 0;
            }

            //create output nodes
            for (int i = 0; i < outputs; i++)
            {
                Nodes.Add(new NeuralNode(i + inputs));
                Nodes[i].Layer = 1;
                NextNode++;
            }

            //bias node
            Nodes.Add(new NeuralNode(NextNode));
            BiasNode = NextNode;
            NextNode++;
            Nodes[BiasNode].Layer = 0;
        }

        //return node that matches number
        public NeuralNode GetNode(int nodeNumber)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Number == nodeNumber)
                {
                    return Nodes[i];
                }
            }
            return null;
        }

        //Add connection going out of a node so that is can get the next node during the feed formward
        [ExcludeFromCodeCoverage]
        public void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].OutputConnections.Clear();
            }

            for (int i = 0; i < Genes.Count; i++)
            {
                Genes[i].FromNode.OutputConnections.Add(Genes[i]);
            }
        }

        //Feed input values into the neural network en get the output values back
        public float[] FeedForward(float[] inputValues)
        //set output
        {
            for (int i = 0; i < Inputs; i++)
            {
                Nodes[Outputs + i].OutputValue = inputValues[i];
            }
            Nodes[BiasNode].OutputValue = 1;//output of bias is 1

            for (int i = 0; i < Network.Count; i++)
            {
                //for each node in the network engage it(see node class for what this does)
                Network[i].Engage();
            }

            //the outputs are nodes[inputs] to nodes [inputs+outputs-1]
            float[] outs = new float[Outputs];
            for (int i = 0; i < Outputs; i++)
            {
                outs[i] = Nodes[i].OutputValue;
            }

            for (int i = 0; i < Nodes.Count; i++)
            {
                //reset all the nodes for the next feed forward
                Nodes[i].InputSum = 0;
            }

            return outs;
        }

        //sets up the NN as a list of nodes in order to be engaged

        public void GenerateNetwork()
        {
            ConnectNodes();
            Network = new List<NeuralNode>();
            //for each layer add the node in that layer, since layers cannot connect to themselves there is no need to order the nodes within a layer

            for (int l = 0; l < Layers; l++)
            {
                //for each layer
                for (int i = 0; i < Nodes.Count; i++)
                {
                    //for each node
                    if (Nodes[i].Layer == l)
                    {
                        //if that node is in that layer
                        Network.Add(Nodes[i]);
                    }
                }
            }
        }

        //mutate the NN by adding a new node
        //it does this by picking a random connection and disabling it then 2 new connections are added
        //1 between the input node of the disabled connection and the new node
        //and the other between the new node and the output of the disabled connection
        [ExcludeFromCodeCoverage]
        public void AddNode(List<ConnectionHistory> innovationHistory)
        {
            //pick a random connection to create a node between
            if (Genes.Count == 0)
            {
                AddConnection(innovationHistory);
                return;
            }
            int randomConnection = (_random.Next(Genes.Count));

            while (Genes[randomConnection].FromNode == Nodes[BiasNode] && Genes.Count != 1)
            {
                //dont disconnect bias
                randomConnection = (_random.Next(Genes.Count));
            }

            Genes[randomConnection].Enabled = false;//disable it

            int newNodeNo = NextNode;
            Nodes.Add(new NeuralNode(newNodeNo));
            NextNode++;
            //add a new connection to the new node with a weight of 1
            int connectionInnovationNumber = GetInnovationNumber(innovationHistory, Genes[randomConnection].FromNode, GetNode(newNodeNo));
            Genes.Add(new ConnectionGene(Genes[randomConnection].FromNode, GetNode(newNodeNo), 1, connectionInnovationNumber));

            connectionInnovationNumber = GetInnovationNumber(innovationHistory, GetNode(newNodeNo), Genes[randomConnection].ToNode);
            //add a new connection from the new node with a weight the same as the disabled connection
            Genes.Add(new ConnectionGene(GetNode(newNodeNo), Genes[randomConnection].ToNode, Genes[randomConnection].Weight, connectionInnovationNumber));
            GetNode(newNodeNo).Layer = Genes[randomConnection].FromNode.Layer + 1;

            connectionInnovationNumber = GetInnovationNumber(innovationHistory, Nodes[BiasNode], GetNode(newNodeNo));
            //connect the bias to the new node with a weight of 0
            Genes.Add(new ConnectionGene(Nodes[BiasNode], GetNode(newNodeNo), 0, connectionInnovationNumber));

            //if the layer of the new node is equal to the layer of the output node of the old connection then a new layer needs to be created
            //more accurately the layer numbers of all layers equal to or greater than this new node need to be incrimented
            if (GetNode(newNodeNo).Layer == Genes[randomConnection].ToNode.Layer)
            {
                for (int i = 0; i < Nodes.Count - 1; i++)
                {
                    //dont include this newest node
                    if (Nodes[i].Layer >= GetNode(newNodeNo).Layer)
                    {
                        Nodes[i].Layer++;
                    }
                }
                Layers++;
            }
            ConnectNodes();
        }

        //adds a connection between 2 nodes which aren't currently connected
        [ExcludeFromCodeCoverage]
        public void AddConnection(List<ConnectionHistory> innovationHistory)
        {
            //cannot add a connection to a fully connected network
            if (FullyConnected())
            {
                Console.WriteLine("connection failed");
                return;
            }

            //get random nodes
            int randomNode1 = (_random.Next(Nodes.Count));
            int randomNode2 = (_random.Next(Nodes.Count));
            while (BadConnectionNodeHasBeenMade(randomNode1, randomNode2))
            {
                //while the random nodes are no good
                //get new ones
                randomNode1 = (_random.Next(Nodes.Count));
                randomNode2 = (_random.Next(Nodes.Count));
            }
            int temp;
            if (Nodes[randomNode1].Layer > Nodes[randomNode2].Layer)
            {
                //if the first random node is after the second then switch
                temp = randomNode2;
                randomNode2 = randomNode1;
                randomNode1 = temp;
            }

            //get the innovation number of the connection
            //this will be a new number if no identical genome has mutated in the same way
            int connectionInnovationNumber = GetInnovationNumber(innovationHistory, Nodes[randomNode1], Nodes[randomNode2]);
            //add the connection with a random array

            Genes.Add(new ConnectionGene(Nodes[randomNode1], Nodes[randomNode2], _random.Next(-1, 1), connectionInnovationNumber));//changed this so if error here
            ConnectNodes();
        }

        [ExcludeFromCodeCoverage]
        public bool BadConnectionNodeHasBeenMade(int r1, int r2)
        {
            if (Nodes[r1].Layer == Nodes[r2].Layer) return true; // if the nodes are in the same layer
            if (Nodes[r1].IsConnectedTo(Nodes[r2])) return true; //if the nodes are already connected

            return false;
        }

        //returns the innovation number for the new mutation
        //if this mutation has never been seen before then it will be given a new unique innovation number
        //if this mutation matches a previous mutation then it will be given the same innovation number as the previous one
        [ExcludeFromCodeCoverage]
        public int GetInnovationNumber(List<ConnectionHistory> innovationHistory, NeuralNode from, NeuralNode to)
        {
            bool isNew = true;
            int connectionInnovationNumber = NextConnectionNo;
            for (int i = 0; i < innovationHistory.Count; i++)
            {
                //for each previous mutation
                if (innovationHistory[i].Matches(this, from, to))
                {
                    //if match found
                    isNew = false;//its not a new mutation
                    connectionInnovationNumber = innovationHistory[i].InnovationNumber; //set the innovation number as the innovation number of the match
                    break;
                }
            }

            if (isNew)
            {
                //if the mutation is new then create an arrayList of integers representing the current state of the genome
                List<int> innoNumbers = new List<int>();
                for (int i = 0; i < Genes.Count; i++)
                {
                    //set the innovation numbers
                    innoNumbers.Add(Genes[i].InnovationNo);
                }

                //then add this mutation to the innovationHistory
                innovationHistory.Add(new ConnectionHistory(from.Number, to.Number, connectionInnovationNumber, innoNumbers));
                NextConnectionNo++;
            }
            return connectionInnovationNumber;
        }

        //returns whether the network is fully connected or not
        [ExcludeFromCodeCoverage]
        public bool FullyConnected()
        {
            int maxConnections = 0;
            int[] nodesInLayers = new int[Layers];//array which stored the amount of nodes in each layer

            //populate array
            for (int i = 0; i < Nodes.Count; i++)
            {
                nodesInLayers[Nodes[i].Layer] += 1;
            }

            //for each layer the maximum amount of connections is the number in this layer * the number of nodes infront of it
            //so lets add the max for each layer together and then we will get the maximum amount of connections in the network
            for (int i = 0; i < Layers - 1; i++)
            {
                int nodesInFront = 0;
                for (int j = i + 1; j < Layers; j++)
                {
                    //for each layer infront of this layer
                    nodesInFront += nodesInLayers[j];//add up nodes
                }

                maxConnections += nodesInLayers[i] * nodesInFront;
            }

            if (maxConnections == Genes.Count)
            {
                //if the number of connections is equal to the max number of connections possible then it is full
                return true;
            }
            return false;
        }

        //mutates the genome
        [ExcludeFromCodeCoverage]
        public void Mutate(List<ConnectionHistory> innovationHistory)
        {
            if (Genes.Count == 0)
            {
                AddConnection(innovationHistory);
            }

            float rand1 = (float)(_random.NextDouble());
            if (rand1 < 0.8)
            {
                // 80% of the time mutate weights
                for (int i = 0; i < Genes.Count; i++)
                {
                    Genes[i].MutateWeight();
                }
            }
            //5% of the time add a new connection
            float rand2 = (float)(_random.NextDouble());
            if (rand2 < 0.05)
            {
                AddConnection(innovationHistory);
            }

            //1% of the time add a node
            float rand3 = (float)(_random.NextDouble());
            if (rand3 < 0.01)
            {
                AddNode(innovationHistory);
            }
        }

        //called when this Genome is better that the other parent
        [ExcludeFromCodeCoverage]
        public Genome Crossover(Genome parent2)
        {
            Genome child = new Genome(Inputs, Outputs, true);
            child.Genes.Clear();
            child.Nodes.Clear();
            child.Layers = Layers;
            child.NextNode = NextNode;
            child.BiasNode = BiasNode;
            List<ConnectionGene> childGenes = new List<ConnectionGene>();//list of genes to be inherrited form the parents
            List<bool> isEnabled = new List<bool>();
            //all inherrited genes
            for (int i = 0; i < Genes.Count; i++)
            {
                bool setEnabled = true;//is this node in the chlid going to be enabled

                int parent2gene = MatchingGene(parent2, Genes[i].InnovationNo);
                if (parent2gene != -1)
                {
                    //if the genes match
                    if (!Genes[i].Enabled || !parent2.Genes[parent2gene].Enabled && ((float)_random.NextDouble() < 0.75))
                    {
                        //if either of the matching genes are disabled
                        //75% of the time disabel the childs gene
                        setEnabled = false;
                    }
                    float rand = (float)_random.NextDouble();
                    if (rand < 0.5)
                    {
                        childGenes.Add(Genes[i]);

                        //get gene from this respectable creature
                    }
                    else
                    {
                        //get gene from parent2
                        childGenes.Add(parent2.Genes[parent2gene]);
                    }
                }
                else
                {
                    //disjoint or excess gene
                    childGenes.Add(Genes[i]);
                    setEnabled = Genes[i].Enabled;
                }
                isEnabled.Add(setEnabled);
            }

            //since all excess and disjoint genes are inherrited from the more fit parent (this Genome) the childs structure is no different from this parent | with exception of dormant connections being enabled but this wont effect nodes
            //so all the nodes can be inherrited from this parent
            for (int i = 0; i < Nodes.Count; i++)
            {
                child.Nodes.Add(Nodes[i].Clone());
            }

            //clone all the connections so that they connect the childs new nodes

            for (int i = 0; i < childGenes.Count; i++)
            {
                child.Genes.Add(childGenes[i].Clone(child.GetNode(childGenes[i].FromNode.Number), child.GetNode(childGenes[i].ToNode.Number)));
                child.Genes[i].Enabled = isEnabled[i];
            }

            child.ConnectNodes();
            return child;
        }

        //create an empty genome
        [ExcludeFromCodeCoverage]
        public Genome(int input, int output, bool crossover)
        {
            //set input number and output number
            Inputs = input;
            Outputs = output;
        }

        //returns whether or not there is a gene matching the input innovation number  in the input genome
        public static int MatchingGene(Genome parent2, int innovationNumber)
        {
            for (int i = 0; i < parent2.Genes.Count; i++)
            {
                if (parent2.Genes[i].InnovationNo == innovationNumber)
                {
                    return i;
                }
            }
            return -1; //no matching gene found
        }

        //returns a copy of this genome
        [ExcludeFromCodeCoverage]
        public Genome Clone()
        {
            Genome clone = new Genome(Inputs, Outputs, true);

            for (int i = 0; i < Nodes.Count; i++)
            {
                //copy nodes
                clone.Nodes.Add(Nodes[i].Clone());
            }

            //copy all the connections so that they connect the clone new nodes

            for (int i = 0; i < Genes.Count; i++)
            {
                //copy genes
                clone.Genes.Add(Genes[i].Clone(clone.GetNode(Genes[i].FromNode.Number), clone.GetNode(Genes[i].ToNode.Number)));
            }

            clone.Layers = Layers;
            clone.NextNode = NextNode;
            clone.BiasNode = BiasNode;
            clone.ConnectNodes();

            return clone;
        }

        //prints out info about the genome to the console
        [ExcludeFromCodeCoverage]
        public void PrintGenome()
        {
            Console.Clear();
            Console.WriteLine("Print genome  layers: " + Layers);
            Console.WriteLine("bias node: " + BiasNode);
            Console.WriteLine("nodes");
            for (int i = 0; i < Nodes.Count; i++)
            {
                Console.WriteLine(Nodes[i].Number + " is : " + GetNodeName(Nodes[i].Number));
            }
            Console.WriteLine("Genes");
            for (int i = 0; i < Genes.Count; i++)
            {
                //for each connectionGene
                Console.WriteLine("gene " + Genes[i].InnovationNo + " From node " + GetNodeName(Genes[i].FromNode.Number) + " To node " + GetNodeName(Genes[i].ToNode.Number) +
                  " is enabled " + Genes[i].Enabled + " from layer " + Genes[i].FromNode.Layer + " to layer " + Genes[i].ToNode.Layer + " weight: " + Genes[i].Weight);
            }

            Console.WriteLine();
        }

        [ExcludeFromCodeCoverage]
        public string GetNodeName(int nodenum)
        {
            switch (nodenum)
            {
                case 0:
                    return "Creature X pos";
                    break;

                case 1:
                    return "Creature Y pos";
                    break;

                case 2:
                    return "Creature damage";
                    break;

                case 3:
                    return "Creature Health";
                    break;

                case 4:
                    return "Distance to closest player";
                    break;

                case 5:
                    return "Distance to closest monster";
                    break;

                case 6:
                    return "Closest player health";
                    break;

                case 7:
                    return "Closest player damage";
                    break;

                case 8:
                    return "Closest player X pos";
                    break;

                case 9:
                    return "Closest player Y pos";
                    break;

                case 10:
                    return "Closest Monster Health";
                    break;

                case 11:
                    return "Closest Monster damage";
                    break;

                case 12:
                    return "Closest Monster X pos";
                    break;

                case 13:
                    return "Closest Monster Y pos";
                    break;

                case 14:
                    return "Attack ction";
                    break;

                case 15:
                    return "Flee action";
                    break;

                case 16:
                    return "Run to monster action";
                    break;

                case 17:
                    return "walk up";
                    break;

                case 18:
                    return "walk down";
                    break;

                case 19:
                    return "walk left";
                    break;

                case 20:
                    return "walk right";
                    break;

                case 21:
                    return "Bias node";
                    break;

                default:
                    return "hiddennode";
            }
        }
    }
}