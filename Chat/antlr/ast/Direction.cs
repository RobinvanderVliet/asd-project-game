/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Direction class for direction of player.
     
*/

using System;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast
{
    public class Direction : ASTNode, IEquatable<Direction>
    {
        public string value;
        
        public Direction(string value)
        {
            this.value = value;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Direction);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Direction other)
        {
            if (other == null)
                return false;

            return value == other.value;
        }
    }
}