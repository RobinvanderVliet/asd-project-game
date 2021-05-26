using System.Collections.Generic;

namespace Agent.Antlr.Ast
{
    public class When : Node
    {
        private Comparable _comparableL;
        public Comparable ComparableL {get=> _comparableL;}
        private Comparable _comparableR;
        public Comparable ComparableR {get => _comparableR;}
        private Comparison _comparison;
        public Comparison Comparison {get => _comparison;}
        private ActionReference _then;
        public ActionReference Then {get => _then;}

        public override string GetNodeType()
        {
            return "When";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();

            if (_comparableL != null)
                children.Add(_comparableL);
            if (_comparison != null)
                children.Add(_comparison);
            if (_comparableR != null)
                children.Add(_comparableR);
            if (_then != null)
                children.Add(_then);

            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node)
            {
                case Comparable comparable:
                    if (_comparableL == null) 
                    {
                        _comparableL = comparable;
                    }
                    else if (_comparableR == null) 
                    {
                        _comparableR = comparable;
                    }
                    else
                    {
                        body.Add(comparable);
                    }
                    break;
                case ActionReference action:
                    _then = action;
                    break;
                case Comparison comparison:
                    _comparison = comparison;
                    break;
                default:
                    body.Add(node);
                    break;
            }

            return this;
        }
    }
}