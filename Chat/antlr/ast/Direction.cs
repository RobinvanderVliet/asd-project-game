/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Direction class for direction of player.
     
*/

using System;

namespace Chat.antlr.ast
{
    public class Direction : ASTNode, IEquatable<Direction>
    {
        private string value;

        public Direction()
        {
        }

        public Direction(string value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Direction);
        }

        public bool Equals(Direction other)
        {
            if (other == null)
                return false;

            return value == other.value;
        }
    }
}