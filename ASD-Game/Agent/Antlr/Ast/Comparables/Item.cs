using System.Collections.Generic;

namespace ASD_project.Agent.Antlr.Ast.Comparables
{

    public class Item : Comparable
    {

        private Stat _stat;

        public string Name { get; set; }

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
            if (_stat != null)
            {
                children.Add(_stat);
            }
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is Stat stat)
            {
                _stat = stat;
            }
            else
            {
                body.Add(node);
            }
            return this;
        }
    }
}