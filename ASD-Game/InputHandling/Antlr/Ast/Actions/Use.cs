using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class Use : Command, IEquatable<Use>
    {
        public Step Step { get; set; }

        public Use()
        {

        }

        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(Step);
            return children;
        }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Step)
            {
                Step = (Step)child;
            }
            return this;
        }

        public ASTNode RemoveChild(ASTNode child)
        {
            if (child is Step && child == Step)
            {
                Step = null;
            }
            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Use);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Use other)
        {
            if (other == null)
            {
                return false;
            }
            return Step.Equals(other.Step);
        }
    }
}
