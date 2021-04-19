/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Step class.
     
*/
namespace Chat.antlr.ast
{
    public class Step : ASTNode
    {
        private int value = 1;

        public Step()
        {
        }

        public Step(int value)
        {
            this.value = value;
        }
    }
}