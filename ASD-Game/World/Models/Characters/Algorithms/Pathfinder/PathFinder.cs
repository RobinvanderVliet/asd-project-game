using System;
using System.Collections.Generic;
using System.Numerics;

namespace ASD_Game.World.Models.Characters.Algorithms.Pathfinder
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

        private void resetGrid()
        {
            for (int row = 0; row < _gridRows; row++)
            {
                for (int col = 0; col < _gridCols; col++)
                {
                    _grid[row][col].Position = new Vector2(row, col);
                    _grid[row][col].DistanceToTarget = -1;
                    _grid[row][col].Cost = 1;
                    _grid[row][col].Parent = null;
                }
            }
        }

        public Stack<Node> FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            Node startNode = new Node(new Vector2((int)startPosition.X, (int)(startPosition.Y)), true);
            Node endNode = new Node(new Vector2((int)endPosition.X, (int)(endPosition.Y)), true);

            Stack<Node> pathStack = new Stack<Node>();

            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            List<Node> adjacencies;

            resetGrid();

            Node currentNode = startNode;

            openList.Add(currentNode);

            while (openList.Count > 0 && !AreNodesAtSamePosition(openList[0], endNode))
            {
                openList.RemoveAt(0);
                closedList.Add(currentNode);

                adjacencies = GetAdjacentNodes(currentNode);

                foreach (Node adjNode in adjacencies)
                {
                    if (adjNode.IsWalkable)
                    {
                        if (!openList.Contains(adjNode))
                        {
                            if (!closedList.Contains(adjNode))
                            {
                                adjNode.Parent = currentNode;
                                adjNode.DistanceToTarget = Math.Abs(adjNode.Position.X - endNode.Position.X) + Math.Abs(adjNode.Position.Y - endNode.Position.Y);
                                adjNode.Cost = adjNode.Weight + adjNode.Parent.Cost;
                                openList.Add(adjNode);
                            }
                            else if (adjNode.FScore > (adjNode.Weight + adjNode.Parent.Cost + adjNode.DistanceToTarget))
                            {
                                adjNode.Parent = currentNode;
                                adjNode.DistanceToTarget = Math.Abs(adjNode.Position.X - endNode.Position.X) + Math.Abs(adjNode.Position.Y - endNode.Position.Y);
                                adjNode.Cost = adjNode.Weight + adjNode.Parent.Cost;
                                closedList.Remove(adjNode);
                                openList.Add(adjNode);
                            }
                        }
                    }
                }
                if (openList.Count > 0)
                {
                    currentNode = openList[0];
                }
            }

            if (!AreNodesAtSamePosition(currentNode, endNode))
            {
                //TODO need to call a destruction action on the stuck monster
                return null;
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
            if (a != null && b != null)
            {
                return a.Position.X.Equals(b.Position.X) && a.Position.Y.Equals(b.Position.Y);
            }
            else
            {
                return false;
            }
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