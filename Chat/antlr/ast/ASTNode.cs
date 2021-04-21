/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: AST node class.
     
*/

using System.Collections;

namespace Chat.antlr.ast
{
    public class ASTNode
    {
        public virtual ArrayList getChildren()
        {
            return new ArrayList();
        }

        public virtual ASTNode addChild(ASTNode child)
        {
            return this;
        }

        public virtual ASTNode removeChild(ASTNode child)
        {
            return this;
        }
    }
}