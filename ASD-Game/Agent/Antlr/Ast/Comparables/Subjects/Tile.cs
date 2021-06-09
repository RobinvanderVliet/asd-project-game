namespace ASD_Game.Agent.Antlr.Ast.Comparables.Subjects
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