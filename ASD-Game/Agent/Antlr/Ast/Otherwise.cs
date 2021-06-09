using System.Collections.Generic;

namespace ASD_Game.Agent.Antlr.Ast
{
    public class Otherwise : Node
    {
        public ActionReference Action;

        public override string GetNodeType()
        {
            return "Otherwise";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();
            if(Action!= null){
                children.Add(Action);
            };
            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is ActionReference actionReference) {
                Action = actionReference;
            }
            else
            {
                body.Add(node);
            }
            return this;
        }
    }
}