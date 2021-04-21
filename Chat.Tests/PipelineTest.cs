using System;
using System.Diagnostics.CodeAnalysis;
using Chat.antlr;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.exception;
using NUnit.Framework;

namespace Chat.Tests
{
    [ExcludeFromCodeCoverage]
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


            moveForward.AddChild(new Move()
                .AddChild(new Direction(direction))
                .AddChild(new Step(steps)));

            return new AST(moveForward);
        }
    }
}