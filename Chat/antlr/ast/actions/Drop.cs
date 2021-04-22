using System;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast.actions
{
    public class Drop : Command, IEquatable<Drop>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Drop);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Drop other)
        {
            return true;
        }
    }
}