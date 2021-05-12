using System;
using System.Numerics;

namespace Creature.Pathfinder
{
    public class Node : IComparable<Node>
    {
        public const int nodeSize = 1;
        public Node parent;
        public Vector2 position;
        public float distanceToTarget;
        public float cost;
        public float weight;
        public float FScore
        {
            get
            {
                if (distanceToTarget != -1 && cost != -1)
                    return distanceToTarget + cost;
                else
                    return -1;
            }
        }
        public bool isWalkable;
        public Node(Vector2 pos, bool isWalkable, float weight = 1)
        {
            this.parent = null;
            this.position = pos;
            this.distanceToTarget = -1;
            this.cost = 1;
            this.isWalkable = isWalkable;
            this.weight = weight;
        }

        public int CompareTo(Node rhs)
        {
            double otherFScore = rhs.FScore;
            return FScore < otherFScore ? -1 : FScore > otherFScore ? 1 : 0;
        }

        //public int CompareTo(Node other) 
        //{ 
        //    int number = FScore.CompareTo(other.FScore);
        //    return number;
        //}

    }
}
