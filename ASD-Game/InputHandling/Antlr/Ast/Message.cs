using System;

namespace ASD_Game.InputHandling.Antlr.Ast
{
    public class Message : ASTNode, IEquatable<Message>
    {
        private string _message;
        public string MessageValue { get => _message; private set => _message = value; }

        public Message()
        {
        }

        public Message(string message)
        {
            _message = message.Replace("\"", "");
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Message);
        }

        public bool Equals(Message other)
        {
            if (other == null)
                return false;

            return _message == other.MessageValue;
        }
        public override int GetHashCode()
        {
            return _message.GetHashCode();
        }
    }
}