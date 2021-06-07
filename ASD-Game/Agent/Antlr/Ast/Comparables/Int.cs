namespace ASD_project.Agent.Antlr.Ast.Comparables
{
    public class Int : Comparable
    {
        public int Value { get; set; }

        public Int(int value)
        {
            Value = value;
        }

        public override string GetNodeType()
        {
            return "Int";
        }
    }
}