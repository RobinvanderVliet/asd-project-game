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
            if (this._whenClause != null)
                children.Add(this._whenClause);
            if (this._otherwiseClause != null)
                children.Add(this._otherwiseClause);
            children.AddRange(this.body);

            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node)
            {
                case When whenClause:
                    this._whenClause = whenClause;
                    break;
                case Otherwise otherwiseClause:
                    this._otherwiseClause = otherwiseClause;
                    break;
                default:
                    this.body.Add(node);
                    break;
            }

            return this;
        }

        public override Node RemoveChild(Node node)
        {
            switch (node)
            {
                case When:
                    this._whenClause = null;
                    break;
                case Otherwise:
                    this._otherwiseClause = null;
                    break;
                default:
                    this.body.Remove(node);
                    break;
            }

            return this;
        }

        public When GetWhenClause()
        {
            return this._whenClause;
        }

        public Otherwise GetOtherWiseClause()
        {
            return this._otherwiseClause;
        }
    }
}