/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Pause action class (extends command).
     
*/

using System;

namespace Chat.antlr.ast.actions
{
    public class Pause : Command, IEquatable<Pause>
    {
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Pause);
        }

        public bool Equals(Pause other)
        {
            return true;
        }
    }
}