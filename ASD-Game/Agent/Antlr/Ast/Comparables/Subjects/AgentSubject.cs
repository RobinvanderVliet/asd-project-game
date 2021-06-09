namespace ASD_Game.Agent.Antlr.Ast.Comparables.Subjects
{

    public class AgentSubject : Subject
    {
        public AgentSubject(string name) : base(name)
        {
        }


        public override string GetNodeType()
        {
            return "Agent";
        }
    }
}