using System.Collections.Generic;

namespace ASD_Game.Agent.Antlr.Ast
{
    public class Condition : Node
    {
        public When WhenClause;
        public Otherwise OtherwiseClause;
        
        public override string GetNodeType()
        {
            return "Condition";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            if (WhenClause != null)
                children.Add(WhenClause);
            if (OtherwiseClause != null)
                children.Add(OtherwiseClause);
            children.AddRange(body);

            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node)
            {
                case When whenClause:
                    WhenClause = whenClause;
                    break;
                case Otherwise otherwiseClause:
                    OtherwiseClause = otherwiseClause;
                    break;
                default:
                    body.Add(node);
                    break;
            }

            return this;
        }

        public override Node RemoveChild(Node node)
        {
            switch (node)
            {
                case When:
                    WhenClause = null;
                    break;
                case Otherwise:
                    OtherwiseClause = null;
                    break;
                default:
                    body.Remove(node);
                    break;
            }

            return this;
        }
    }
}