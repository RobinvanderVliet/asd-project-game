namespace ASD_Game.Agent.Antlr.Ast.Comparables.Subjects
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