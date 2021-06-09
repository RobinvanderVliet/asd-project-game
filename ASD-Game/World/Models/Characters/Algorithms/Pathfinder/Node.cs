using System;
using System.Numerics;

namespace ASD_Game.World.Models.Characters.Algorithms.Pathfinder
{
    public class Node : IComparable<Node>
    {
        private Node _parent;
        private Vector2 _position;

        private float _distanceToTarget;
        private float _cost;
        private float _weight;
        private bool _isWalkable;

        public Node Parent
        {
            get => _parent;
            set => _parent = value;
        }
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }
        public float DistanceToTarget
        {
            get => _distanceToTarget;
            set => _distanceToTarget = value;
        }
        public float Cost
        {
            get => _cost;
            set => _cost = value;
        }
        public float Weight
        {
            get => _weight;
            set => _weight = value;
        }
        public float FScore
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                return -1;
            }
        }
        public bool IsWalkable
        {
            get => _isWalkable;
            set => _isWalkable = value;
        }

        public Node(Vector2 pos, bool isWalkable, float weight = 1)
        {
            _parent = null;
            _position = pos;
            _distanceToTarget = -1;
            _cost = 1;
            _isWalkable = isWalkable;
            _weight = weight;
        }

        public int CompareTo(Node rhs)
        {
            double otherFScore = rhs.FScore;
            return FScore < otherFScore ? -1 : FScore > otherFScore ? 1 : 0;
        }
    }
}