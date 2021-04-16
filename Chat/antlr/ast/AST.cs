/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: AST class.
     
*/
namespace Chat.antlr.ast
{
    public class AST
    {
        public Input root;

        public AST()
        {
            root = new Input();
        }

        public AST(Input input)
        {
            root = input;
        }

        public void setRoot(Input input)
        {
            root = input;
        }
    }
}