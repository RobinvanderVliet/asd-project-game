using System.Collections.Generic;

namespace ASD_Game.Agent.Antlr.Ast.Comparables
{

    public class Item : Comparable
    {

        public Stat Stat;
        public readonly string Name;
        
        public Item(string name)
        {
            Name = name;
        }

        public override string GetNodeType()
        {
            return "Item";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            if (Stat != null) {
                children.Add(Stat);
            }
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is Stat stat) {
                Stat = stat;
            }
            else
            {
                body.Add(node);
            }
            return this;
        }
    }
}