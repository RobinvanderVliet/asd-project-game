namespace ASD_Game.Agent.Antlr.Ast
{

    public class Comparison : Node
    {
        public string ComparisonType { get; set; }

        //TODO: Create comparisonTypeEnum ,(reactie) voor nu overbodig kan later
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