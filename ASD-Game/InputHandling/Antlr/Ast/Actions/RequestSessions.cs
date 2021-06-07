using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace InputHandling.Antlr.Ast.Actions
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