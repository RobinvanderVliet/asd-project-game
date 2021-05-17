using Creature.Exceptions;
using Creature.Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Creature
{
    public class PathFinder
    {
        private List<List<Node>> _grid;
        public PathFinder(List<List<Node>> nodes)
        {
            _grid = nodes;
        }
        int _gridRows
        {
            get
            {
                return _grid[0].Count;
            }
        }
        int _gridCols
        {
            get
            {
                return _grid.Count;
            }
        }

        public Stack<Node> FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            Node startNode = new Node(new Vector2((int)(startPosition.X / Node.nodeSize), (int)(startPosition.Y / Node.nodeSize)), true);
            Node endNode = new Node(new Vector2((int)(endPosition.X / Node.nodeSize), (int)(endPosition.Y / Node.nodeSize)), true);

            Stack<Node> pathStack = new Stack<Node>();

            PriorityQueue<Node> openList = new PriorityQueue<Node>();
            List<Node> closedList = new List<Node>();

            List<Node> adjacencies;
            Node currentNode = startNode;

            openList.Enqueue(currentNode);

            while (openList.Count > 0)
            {
                currentNode = openList.Dequeue();

                if (currentNode.position.X.Equals(endNode.position.X) && currentNode.position.Y.Equals(endNode.position.Y))
                {
                    break;
                }

                //closedList.Add(currentNode);

                adjacencies = GetAdjacentNodes(currentNode);

                foreach (Node adjNode in adjacencies)
                {
                    if ((!closedList.Contains(adjNode) && adjNode.isWalkable) || currentNode.FScore < adjNode.FScore)
                    {
                        closedList.Add(adjNode);

                        //if (!openList.Contains(adjNode))
                        //{
                            adjNode.parent = currentNode;
                            adjNode.distanceToTarget = Math.Abs(adjNode.position.X - endNode.position.X) + Math.Abs(adjNode.position.Y - endNode.position.Y);
                            adjNode.cost = adjNode.weight + adjNode.parent.cost;
                            openList.Enqueue(adjNode);
                        //}
                            
                    }
                }
            }

            if (currentNode == null)
                return null;

            if (currentNode.position.X != endNode.position.X && currentNode.position.Y != endNode.position.Y)
                throw new PathHasNoDestinationException();

            if (currentNode == startNode)
                pathStack.Push(currentNode);

            while (currentNode != startNode && currentNode != null)
            {
                pathStack.Push(currentNode);
                currentNode = currentNode.parent;
            }

            return pathStack;
        }

        private List<Node> GetAdjacentNodes(Node node)
        {
            List<Node> temp = new List<Node>();

            int row = (int)node.position.Y;
            int col = (int)node.position.X;

            if (row + 1 < _gridRows)
            {
                temp.Add(_grid[col][row + 1]);
            }
            if (row - 1 >= 0)
            {
                temp.Add(_grid[col][row - 1]);
            }
            if (col - 1 >= 0)
            {
                temp.Add(_grid[col - 1][row]);
            }
            if (col + 1 < _gridCols)
            {
                temp.Add(_grid[col + 1][row]);
            }

            return temp;
        }
    }
}
