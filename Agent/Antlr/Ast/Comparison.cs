namespace Agent.Antlr.Ast
{
   
    public class Comparison : Node
    {
        public string ComparisonType { get; set; }
        
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