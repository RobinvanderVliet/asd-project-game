using System;

namespace InputCommandHandler.Antlr.Ast
{
    public class Message : ASTNode, IEquatable<Message>
    {
        private string _message;
        public string MessageValue { get => this._message; private set => this._message = value; }

        public Message()
        {
        }

        public Message(string message)
        {
            this._message = message.Replace("\"", "");
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Message);
        }

        public bool Equals(Message other)
        {
            if (other == null)
                return false;

            return this._message == other._message;
        }
    }
}