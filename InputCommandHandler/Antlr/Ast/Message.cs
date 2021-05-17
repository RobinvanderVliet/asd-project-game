using System;

namespace InputCommandHandler.Antlr.Ast
{
    public class Message : ASTNode, IEquatable<Message>
    {
        private string _value;
        public string Value { get => _value; private set => _value = value; }

        public Message()
        {
        }

        public Message(string value)
        {
            _value = value.Replace("\"", "");
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Message);
        }

        public bool Equals(Message other)
        {
            if (other == null)
                return false;

            return _value == other._value;
        }
    }
}