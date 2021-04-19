using Agent.antlr.ast.interfaces;
using System.Collections;

namespace Agent.antlr.ast.implementation
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public class Condition : Node, ICondition
    {
        private When _whenClause;
        private Otherwise _otherwiseClause;

        public new string GetNodeType()
        {
            return "Condition";
        }

        public new ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(_whenClause);
            children.AddRange(body);
            if (_otherwiseClause != null)
                children.Add(_otherwiseClause);
            
            return body;
        }

        public new INode AddChild(INode node)
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
        
        public new INode RemoveChild(INode node)
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