using Agent.antlr.ast.interfaces;
using Agent.antlr.ast.interfaces.comparables;
using System.Collections;

namespace Agent.antlr.ast.implementation.comparables
{
    /*
    AIM SD ASD 2020/2021 S2 project
         
    Project name: [to be determined].

    This file is created by team: 1].
     
    Goal of this file: [making_the_system_work].
         
    */

    public class Item : Comparable, IItem
    {

        private IStat _stat;
        
        public string Name { get; set; }
        
        public Item(string name)
        {
            Name = name;
        }

        public new string GetNodeType()
        {
            return "Item";
        }

        public new ArrayList GetChildren()
        {
            var children = new ArrayList();
            if (this._stat != null) {
                children.Add(this._stat);
            }
            children.AddRange(this.body);
            return children;
        }

        public new INode AddChild(INode node)
        {
            if (node is IStat stat) {
                this._stat = stat;
            }
            else {
                this.body.Add(node);
            }
            return this;
        }
    }
}