using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class RequestSessions : Command, IEquatable<RequestSessions>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as RequestSessions);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(RequestSessions other)
        {
            return true;
        }
    }
}