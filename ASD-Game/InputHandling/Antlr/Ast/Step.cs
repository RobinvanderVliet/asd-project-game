using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast
{
    public class Step : ASTNode, IEquatable<Step>
    {
        private int _step;
        public int StepValue { get => _step; private set => _step = value; }

        public Step()
        {
            _step = 1;
        }

        public Step(int step)
        {
            _step = step;
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Step other)
        {
            if (other == null)
                return false;

            return _step == other.StepValue;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Step);
        }
        public override int GetHashCode()
        {
            return _step.GetHashCode();
        }
    }
}