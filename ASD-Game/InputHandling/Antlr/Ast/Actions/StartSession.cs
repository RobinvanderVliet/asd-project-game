using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class StartSession : Command, IEquatable<StartSession>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as StartSession);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(StartSession other)
        {
            return true;
        }
    }
}