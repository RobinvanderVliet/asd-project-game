namespace ASD_Game.Agent.Antlr.Ast.Comparables.Subjects
{
    public class Inventory : Subject
    {
        public Inventory(string name) : base(name)
        {
        }


        public override string GetNodeType()
        {
            return "Inventory";
        }
    }
}