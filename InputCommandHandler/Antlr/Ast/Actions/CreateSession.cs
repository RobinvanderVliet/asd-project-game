using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast.Actions
{
    public class CreateSession: Command
    {
        private Message _message;
        [ExcludeFromCodeCoverage]
        public Message Message { get => _message; private set => _message = value; }

        [ExcludeFromCodeCoverage]
        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(_message);
            return children;
        }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Message)
            {
                _message = (Message) child;
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