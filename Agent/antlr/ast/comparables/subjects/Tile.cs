namespace Agent.antlr.ast.comparables.subjects
{

    public class Tile : Subject
    {
        public Tile(string name) : base(name)
        {
        }


        public override string GetNodeType()
        {
            return "Tile";
        }
    }
}