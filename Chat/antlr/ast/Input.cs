using System;
using System.Collections;

/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Input class (like command).
     
*/
namespace Chat.antlr.ast
{
    public class Input : ASTNode, IEquatable<Input>
    {
        public ArrayList body;

        public Input()
        {
            this.body = new ArrayList();
        }

        public Input(ArrayList body)
        {
            this.body = body;
        }

        public override ArrayList getChildren()
        {
            return this.body;
        }

        public override ASTNode addChild(ASTNode child)
        {
            body.Add(child);
            return this;
        }

        public override ASTNode removeChild(ASTNode child)
        {
            body.Remove(child);
            return this;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Input);
        }

        public bool Equals(Input other)
        {
            if (other == null)
                return false;

            if (body.Count != other.body.Count)
            {
                return false;
            }
            
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