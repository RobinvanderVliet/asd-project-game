using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast.Actions
{
    public class RequestSessions : Command
    {
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
    }
}