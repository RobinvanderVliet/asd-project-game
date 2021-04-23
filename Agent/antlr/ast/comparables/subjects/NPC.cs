
namespace Agent.antlr.ast.comparables.subjects
{
    public class NPC : Subject
    {
        public NPC(string name) : base(name)
        {
        }

        public override string GetNodeType()
        {
            return "NPC";
        }
    }
}