/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Drop action class (extends command).
     
*/

using System;

namespace Chat.antlr.ast.actions
{
    public class Drop : Command, IEquatable<Drop>
    {
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Drop);
        }

        public bool Equals(Drop other)
        {
            return true;
        }
    }
}