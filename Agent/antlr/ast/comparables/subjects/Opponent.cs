namespace Agent.antlr.ast.comparables.subjects
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