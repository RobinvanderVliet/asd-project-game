/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Direction class for direction of player.
     
*/
namespace Chat.antlr.ast
{
    public class Direction : ASTNode
    {
        private string value;

        public Direction()
        {
        }

        public Direction(string value)
        {
            this.value = value;
        }
    }
}