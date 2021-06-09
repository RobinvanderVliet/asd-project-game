namespace ASD_Game.Agent.Antlr.Ast.Comparables
{
    public class Int : Comparable
    {
        public readonly int Value;
        
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