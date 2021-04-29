using System;

namespace InputCommandHandler.Antlr.Ast
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