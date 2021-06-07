namespace Agent.Antlr.Ast.Comparables.Subjects
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