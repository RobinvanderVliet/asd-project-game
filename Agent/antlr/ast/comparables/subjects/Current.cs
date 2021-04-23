namespace Agent.antlr.ast.comparables.subjects
{
    public class Current : Subject
    {
        public Current(string name) : base(name)
        {
        }


        public override string GetNodeType()
        {
            return "Current";
        }
    }
}