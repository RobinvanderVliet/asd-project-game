namespace ASD_Game.Agent.Antlr.Ast.Comparables.Subjects
{

    public class Player : Subject
    {
        public Player(string name) : base(name)
        {
        }


        public override string GetNodeType()
        {
            return "Player";
        }
    }
}