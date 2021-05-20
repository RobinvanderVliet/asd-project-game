using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    public class Condition : Node
    {
        private When _whenClause;
        private Otherwise _otherwiseClause;

        public override string GetNodeType()
        {
            return "Condition";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            if (_whenClause != null)
                children.Add(_whenClause);
            if (_otherwiseClause != null)
                children.Add(_otherwiseClause);
            children.AddRange(body);

            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node)
            {
                case When whenClause:
                    _whenClause = whenClause;
                    break;
                case Otherwise otherwiseClause:
                    _otherwiseClause = otherwiseClause;
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
                    _whenClause = null;
                    break;
                case Otherwise:
                    _otherwiseClause = null;
                    break;
                default:
                    body.Remove(node);
                    break;
            }

            return this;
        }

        public When GetWhenClause()
        {
            return _whenClause;
        }

        public Otherwise GetOtherWiseClause()
        {
            return _otherwiseClause;
        }
    }
}