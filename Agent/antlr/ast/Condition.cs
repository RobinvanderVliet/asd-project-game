using System.Collections.Generic;

namespace Agent.antlr.ast
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public class Condition : Node
    {
        private When _whenClause;
        private Otherwise _otherwiseClause;

        public new string GetNodeType()
        {
            return "Condition";
        }

        public new List<Node> GetChildren()
        {
            var children = new List<Node>();
            if (_whenClause != null)
                children.Add(_whenClause);
            if (_otherwiseClause != null)
                children.Add(_otherwiseClause);
            children.AddRange(body);

            return children;
        }

        public new Node AddChild(Node node)
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

        public new Node RemoveChild(Node node)
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