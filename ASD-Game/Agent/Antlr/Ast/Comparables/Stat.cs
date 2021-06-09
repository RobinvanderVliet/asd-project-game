
namespace ASD_Game.Agent.Antlr.Ast.Comparables
{

    public class Stat : Comparable
    {
        public readonly string Name;
        
        public Stat(string name)
        {
            Name = name;
        }

        public override string GetNodeType()
        {
            return "Stat";
        }

    }
}