namespace Agent.antlr.ast.comparables
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