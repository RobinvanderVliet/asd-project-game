
namespace Agent.Antlr.Ast.Comparables
{
    public class Stat : Comparable
    {
        public string Name { get; set; }

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