namespace Agent.antlr.ast.comparables.subjects
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