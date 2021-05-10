﻿using Creature.Pathfinder;
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

            Stack<Node> path = new Stack<Node>();
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();
            List<Node> adjacencies;
            Node current = startNode;

            // Add start node to OpenList
            openList.Add(startNode);

            while (openList.Count != 0 && !closedList.Exists(x => x.position == endNode.position))
            {
                current = openList[0];
                openList.Remove(current);
                closedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

                foreach (Node n in adjacencies)
                {
                    if (!closedList.Contains(n) && n.isWalkable)
                    {
                        if (!openList.Contains(n))
                        {
                            n.parent = current;
                            n.distanceToTarget = Math.Abs(n.position.X - endNode.position.X) + Math.Abs(n.position.Y - endNode.position.Y);
                            n.cost = n.weight + n.parent.cost;
                            openList.Add(n);
                            openList = openList.OrderBy(node => node.FScore).ToList<Node>();
                        }
                    }
                }
            }

            // Construct path, if end was not closed return null
            if (!closedList.Exists(x => x.position == endNode.position))
            {
                return null;
            }

            // If the end was reached, return the path
            Node temp = closedList[closedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                path.Push(temp);
                temp = temp.parent;
            } while (temp != startNode && temp != null);
            return path;
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