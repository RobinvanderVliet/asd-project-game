using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast
{
    public class Input : ASTNode, IEquatable<Input>
    {
        public ArrayList body;

        public Input()
        {
            body = new ArrayList();
        }

        

        public override ASTNode AddChild(ASTNode child)
        {
            body.Add(child);
            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Input);
        }
        
        [ExcludeFromCodeCoverage]
        public bool Equals(Input other)
        {
            if (other == null)
                return false;

            if (body.Count != other.body.Count) 
                return false;

            for (int i = 0; i < body.Count; i++)
            {
                if (!body[i].Equals(other.body[i]))
                {
                    return false;
                }
            }
              return true;
        }
        
    }
}