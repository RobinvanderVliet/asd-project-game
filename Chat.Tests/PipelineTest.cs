using System;
using Chat.antlr;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.exception;
using NUnit.Framework;

namespace Chat.Tests
{
    public class PipelineTest
    {
        [Test]
        public void ThrowsSyntaxErrorWhenCommandNotRecognised()
        {
            Pipeline sut = new Pipeline();
            Assert.Throws<CommandSyntaxException>(() => sut.ParseCommand("me forward"));
        }
        
        [Test]
        public void change()
        {
            Pipeline sut = new Pipeline();
            sut.ParseCommand("move forward");
            AST ast = sut.Ast;
            AST exp = MoveCommand(1, "forward");

            Assert.AreEqual(exp, ast);
        }
        
        public static AST MoveCommand(int steps, String direction)
        {
            Input moveForward = new Input();


            moveForward.addChild(new Move()
                .addChild(new Direction(direction))
                .addChild(new Step(steps)));

            return new AST(moveForward);
        }
    }
}