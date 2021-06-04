namespace ASD_project.Agent.Antlr.Ast.Comparables.Subjects
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