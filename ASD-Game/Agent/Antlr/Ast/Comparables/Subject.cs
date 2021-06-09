
namespace ASD_Game.Agent.Antlr.Ast.Comparables
{
    public class Subject : Comparable
    {
        public readonly string Name;
        
        public Subject(string name)
        {
            Name = name;
        }

        public override string GetNodeType()
        {
            return "Subject";
        }
    }
}