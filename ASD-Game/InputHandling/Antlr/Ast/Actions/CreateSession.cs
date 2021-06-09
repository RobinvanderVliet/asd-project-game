using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class CreateSession : Command
    {
        private Message _message;
        private Username _username;

        [ExcludeFromCodeCoverage]
        public Message Message { get => _message; private set => _message = value; }
        public Username Username { get => _username; private set => _username = value; }

        [ExcludeFromCodeCoverage]
        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(_message);
            children.Add(_username);
            return children;
        }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Message)
            {
                _message = (Message)child;
            }
            else if (child is Username)
            {
                _username = (Username)child;
            }

            return this;
        }

        [ExcludeFromCodeCoverage]
        public ASTNode RemoveChild(ASTNode child)
        {
            if (child is Message && child == _message)
            {
                _message = null;
            }

            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Drop);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Drop other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return _message.GetHashCode();
        }
    }
}