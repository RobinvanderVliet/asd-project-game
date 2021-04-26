/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Message action class (extends command).
     
*/

using System;

namespace Chat.antlr.ast
{
    public class Message : ASTNode, IEquatable<Message>
    {
        public string value;

        public Message()
        {
        }

        public Message(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Message);
        }

        public bool Equals(Message other)
        {
            if (other == null)
                return false;

            return value == other.value;
        }
    }
}