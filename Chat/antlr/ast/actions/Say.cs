/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Say action class (extends command).
     
*/

using System;
using System.Collections;

namespace Chat.antlr.ast.actions
{
    public class Say : Command, IEquatable<Say>
    {
        public Message message;

        public override ArrayList getChildren()
        {
            var children = new ArrayList();
            children.Add(message);
            return children;
        }

        public override ASTNode addChild(ASTNode child)
        {
            if (child is Message)
            {
                message = (Message) child;
            }

            return this;
        }

        public override ASTNode removeChild(ASTNode child)
        {
            if (child is Message && child == message)
            {
                message = null;
            }

            return this;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Say);
        }

        public bool Equals(Say other)
        {
            if (other == null)
                return false;

            return message.Equals(other.message);
        }
    }
}