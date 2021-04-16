/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Move action class (extends command).
     
*/
using System.Collections;

namespace Chat.antlr.ast.actions
{
    public class Move : Command
    {
        public Direction direction;
        public Step steps;

        public override ArrayList getChildren()
        {
            var children = new ArrayList();
            children.Add(direction);
            children.Add(steps);
            return children;
        }

        public override ASTNode addChild(ASTNode child)
        {
            if (child is Direction)
            {
                direction = (Direction) child;
            }
            else if (child is Step)
            {
                steps = (Step) child;
            }

            return this;
        }

        public override ASTNode removeChild(ASTNode child)
        {
            if (child is Direction && child == direction)
            {
                direction = null;
            }
            else if (child is Step && child == steps)
            {
                steps = null;
            }

            return this;
        }
    }
}