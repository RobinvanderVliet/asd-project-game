using Creature.Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Creature
{
    public class PathFinder
    {
        List<List<Node>> _grid;
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

            Dictionary<Node, Node> currentPath = new Dictionary<Node, Node>();
            Stack<Node> pathStack = new Stack<Node>();

            PriorityQueue<Node> openList = new PriorityQueue<Node>();
            List<Node> closedList = new List<Node>();

            List<Node> adjacencies;
            Node currentNode = startNode;

            openList.Enqueue(currentNode, 0);
            currentPath[startNode] = currentNode;
            closedList.Add(currentNode);

            while (openList.Count > 0 && !(currentNode.position.X.Equals(endNode.position.X) && currentNode.position.Y.Equals(endNode.position.Y)))
            {
                Node lowestCostNode = openList.Dequeue();
                currentPath[lowestCostNode] = currentNode;

                currentNode = lowestCostNode;

                adjacencies = GetAdjacentNodes(currentNode);

                foreach (Node adjNode in adjacencies)
                {
                    if (adjNode.isWalkable && (!closedList.Contains(adjNode) || currentNode.FScore < adjNode.FScore))
                    {
                        if (closedList.Contains(adjNode))
                        {
                            closedList.Remove(adjNode);
                            currentPath.Remove(adjNode);
                        }

                        closedList.Add(adjNode);
                        adjNode.parent = currentNode;
                        adjNode.distanceToTarget = Math.Abs(adjNode.position.X - endNode.position.X) + Math.Abs(adjNode.position.Y - endNode.position.Y);
                        adjNode.cost = adjNode.weight + adjNode.parent.cost;
                        openList.Enqueue(adjNode, adjNode.FScore);
                    }
                }
            }

            if (currentNode == null) 
                return null;

            pathStack.Push(currentNode);

            while (currentNode != startNode && currentNode != null)
            {
                pathStack.Push(currentNode);
                currentNode = currentPath[currentNode];
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
