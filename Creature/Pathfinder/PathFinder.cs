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
        private int _gridRows;
        private int _gridCols;

        public PathFinder(List<List<Node>> nodes)
        {
            _grid = nodes;
            _gridRows = _grid[0].Count;
            _gridCols = _grid.Count;
        }

        public Stack<Node> FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            Node startNode = new Node(new Vector2((int)startPosition.X, (int)(startPosition.Y)), true);
            Node endNode = new Node(new Vector2((int)endPosition.X, (int)(endPosition.Y)), true);

            Stack<Node> pathStack = new Stack<Node>();

            PriorityQueue<Node> openList = new PriorityQueue<Node>();
            List<Node> closedList = new List<Node>();

            List<Node> adjacencies;
            Node currentNode = startNode;

            openList.Enqueue(currentNode);

            while (openList.Count > 0 && !AreNodesAtSamePosition(openList.Peek(), endNode))
            {
                openList.Dequeue();
                closedList.Add(currentNode);

                adjacencies = GetAdjacentNodes(currentNode);

                foreach (Node adjNode in adjacencies)
                {
                    if (adjNode.IsWalkable)
                    {
                        if (!(openList.Contains(adjNode) && currentNode.FScore <= adjNode.FScore))
                        {
                            if (closedList.Contains(adjNode))
                            {
                                if (currentNode.FScore > adjNode.FScore)
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
                currentNode = openList.Peek();
            }

            if (!AreNodesAtSamePosition(currentNode, endNode))
            {
                throw new PathHasNoDestinationException();
            }

            else if (AreNodesAtSamePosition(currentNode, startNode))
            {
                pathStack.Push(currentNode);
            }

            while (!AreNodesAtSamePosition(currentNode, startNode) && currentNode != null)
            {
                pathStack.Push(currentNode);
                currentNode = currentNode.Parent;
            }

            return pathStack;
        }

        private bool AreNodesAtSamePosition(Node a, Node b)
        {
            return a.Position.X.Equals(b.Position.X) && a.Position.Y.Equals(b.Position.Y);
        }

        private List<Node> GetAdjacentNodes(Node node)
        {
            List<Node> adjNodes = new List<Node>();

            int row = (int)node.Position.Y;
            int col = (int)node.Position.X;

            if (row + 1 < _gridRows)
            {
                adjNodes.Add(_grid[col][row + 1]);
            }
            if (row - 1 >= 0)
            {
                adjNodes.Add(_grid[col][row - 1]);
            }
            if (col - 1 >= 0)
            {
                adjNodes.Add(_grid[col - 1][row]);
            }
            if (col + 1 < _gridCols)
            {
                adjNodes.Add(_grid[col + 1][row]);
            }

            return adjNodes;
        }
    }
}
