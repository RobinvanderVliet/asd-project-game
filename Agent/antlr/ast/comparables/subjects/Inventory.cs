namespace Agent.antlr.ast.comparables.subjects
{
    public class Inventory : Subject
    {
        public Inventory(string name) : base(name)
        {
        }


        public override string GetNodeType()
        {
            return "Inventory";
        }
    }
}