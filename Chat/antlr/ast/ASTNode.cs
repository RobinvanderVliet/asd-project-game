/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: AST node class.
     
*/

namespace Chat.antlr.ast
{
    public class ASTNode
    {
        public virtual ASTNode AddChild(ASTNode child)
        {
            return this;
        }
    }
}