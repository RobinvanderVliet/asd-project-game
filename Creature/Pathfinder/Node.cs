using System;
using System.Numerics;

namespace Creature.Pathfinder
{
    public class Node : IComparable<Node>
    {
        public const int NodeSize = 1;
        public Node Parent;
        public Vector2 Position;
        public float DistanceToTarget;
        public float Cost;
        public float Weight;
        public float FScore
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }
        public bool IsWalkable;
        
        public Node(Vector2 pos, bool isWalkable, float weight = 1)
        {
            this.Parent = null;
            this.Position = pos;
            this.DistanceToTarget = -1;
            this.Cost = 1;
            this.IsWalkable = isWalkable;
            this.Weight = weight;
        }

        public int CompareTo(Node rhs)
        {
            double otherFScore = rhs.FScore;
            return FScore < otherFScore ? -1 : FScore > otherFScore ? 1 : 0;
        }

    }
}
