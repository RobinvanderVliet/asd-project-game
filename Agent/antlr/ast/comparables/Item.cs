using System.Collections.Generic;

namespace Agent.antlr.ast.comparables
{
    /*
    AIM SD ASD 2020/2021 S2 project
         
    Project name: [to be determined].

    This file is created by team: 1].
     
    Goal of this file: [making_the_system_work].
         
    */

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
            if (this._stat != null) {
                children.Add(this._stat);
            }
            children.AddRange(this.body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            if (node is Stat stat) {
                this._stat = stat;
            }
            else {
                this.body.Add(node);
            }
            return this;
        }
    }
}