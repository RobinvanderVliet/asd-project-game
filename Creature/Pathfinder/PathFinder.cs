using Creature.Exceptions;
using Creature.Pathfinder;
using System;
using System.Collections.Generic;
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
            Node startNode = new Node(new Vector2((int)(startPosition.X / Node.NodeSize), (int)(startPosition.Y / Node.NodeSize)), true);
            Node endNode = new Node(new Vector2((int)(endPosition.X / Node.NodeSize), (int)(endPosition.Y / Node.NodeSize)), true);

            Stack<Node> pathStack = new Stack<Node>();

            PriorityQueue<Node> openList = new PriorityQueue<Node>();
            List<Node> closedList = new List<Node>();

            List<Node> adjacencies;
            Node currentNode = startNode;

            openList.Enqueue(currentNode);

            while (openList.Count > 0)
            {
                currentNode = openList.Dequeue();
                closedList.Add(currentNode);

                if (currentNode.Position.X.Equals(endNode.Position.X) && currentNode.Position.Y.Equals(endNode.Position.Y))
                {
                    break;
                }

                adjacencies = GetAdjacentNodes(currentNode);

                foreach (Node adjNode in adjacencies)
                {
                    if (adjNode.IsWalkable)
                    {

                        if (openList.Contains(adjNode))
                        {
                            if (currentNode.FScore <= adjNode.FScore)
                            {
                                continue;
                            }
                        }

                        if (closedList.Contains(adjNode))
                        {
                            if (!(currentNode.FScore <= adjNode.FScore))
                            {
                                closedList.Remove(adjNode);
                            }
                        }
                        else
                        {
                            adjNode.Parent = currentNode;
                            adjNode.DistanceToTarget = Math.Abs(adjNode.Position.X - endNode.Position.X) + Math.Abs(adjNode.Position.Y - endNode.Position.Y);
                            adjNode.Cost = adjNode.Weight + adjNode.Parent.Cost;
                            openList.Enqueue(adjNode);
                        }
                            
                    }
                }
            }

            if (currentNode == null)
            {
                return null;
            }

            if (currentNode.Position.X != endNode.Position.X && currentNode.Position.Y != endNode.Position.Y)
            {
                throw new PathHasNoDestinationException();
            }

            if (currentNode == startNode)
            {
                pathStack.Push(currentNode);
            }

            while (currentNode != startNode && currentNode != null)
            {
                pathStack.Push(currentNode);
                currentNode = currentNode.Parent;
            }

            return pathStack;
        }

        private List<Node> GetAdjacentNodes(Node node)
        {
            List<Node> temp = new List<Node>();

            int row = (int)node.Position.Y;
            int col = (int)node.Position.X;

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
