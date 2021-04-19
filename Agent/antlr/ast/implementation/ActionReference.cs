using Agent.antlr.ast.interfaces;
using Agent.antlr.ast.interfaces.comparables;
using System.Collections;

namespace Agent.antlr.ast.implementation
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public class ActionReference : Node, IActionReference
    {
        private ISubject _subject;
        private IItem _item;
        
        
        public string Name { get; set; }
        
        public ActionReference(string name)
        {
            Name = name;
        }


        public new string GetNodeType()
        {
            return "ActionReference";
        }

        public new ArrayList GetChildren()
        {
            var children = new ArrayList() {
                this._item,
                this._subject
            };
            children.AddRange(body);
            return children;
        }

        public new INode AddChild(INode node)
        {
            switch (node) {
                case ISubject subject:
                    this._subject = subject;
                    break;
                case IItem item:
                    this._item = item;
                    break;
                default:
                    this.body.Add(node);
                    break;
            }
            return this;
        }
    }
}