namespace ASD_Game.Agent.Antlr.Ast.Comparables.Subjects
{

    public class Opponent : Subject
    {
        public Opponent(string name) : base(name)
        {
        }


        public override string GetNodeType()
        {
            return "Opponent";
        }
    }
}