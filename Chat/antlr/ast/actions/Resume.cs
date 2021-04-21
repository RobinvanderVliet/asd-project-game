using System;

namespace Chat.antlr.ast.actions
{
    public class Resume : Command, IEquatable<Resume>
    {
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Resume);
        }

        public bool Equals(Resume other)
        {
            return true;
        }
    }
}