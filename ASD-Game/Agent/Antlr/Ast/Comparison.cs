namespace Agent.Antlr.Ast
{

    public class Comparison : Node
    {
        public readonly string ComparisonType;
        
        public Comparison(string comparisonType)
        {
            ComparisonType = comparisonType;
        }

        public override string GetNodeType()
        {
            return "Comparison";
        }
    }
}