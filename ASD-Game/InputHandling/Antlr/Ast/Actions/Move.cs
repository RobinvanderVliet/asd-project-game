using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class Move : Command, IEquatable<Move>
    {
        private Direction _direction;
        public Direction Direction { get => _direction; private set => _direction = value; }
        private Step _steps;
        public Step Steps { get => _steps; private set => _steps = value; }

        public Move()
        {
            _steps = new Step();
        }

        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(Direction);
            children.Add(Steps);
            return children;
        }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Direction)
            {
                Direction = (Direction)child;
            }
            else if (child is Step)
            {
                Steps = (Step)child;
            }

            return this;
        }

        public ASTNode RemoveChild(ASTNode child)
        {
            if (child is Direction && child == Direction)
            {
                Direction = null;
            }
            else if (child is Step && child == Steps)
            {
                Steps = null;
            }

            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Move);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Move other)
        {
            if (other == null)
                return false;

            return Direction.Equals(other.Direction) && Steps.Equals(other.Steps);
        }
        public override int GetHashCode()
        {
            return _direction.GetHashCode();
        }
    }
}