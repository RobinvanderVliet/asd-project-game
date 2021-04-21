using System;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Chat.antlr.ast;
using Chat.antlr.ast.actions;
using Chat.antlr.grammar;
using Chat.antlr.parser;
using Chat.exception;
using NUnit.Framework;
using Moq;

namespace Chat.Tests
{
    [ExcludeFromCodeCoverage]
    public class ChatTest
    {
        private ChatComponent sut;

        // [SetUp]
        // public void Setup()
        // {
        //     sut = new ChatComponent();
        // }
        
        [Test]
        public void TestSendChatException()
        {
            Mock<ChatComponent> mock = new Mock<ChatComponent>();
            mock.Setup(mockedChatComponent => mockedChatComponent.GetCommand()).Returns("move");
            sut = mock.Object;

            Assert.Throws<CommandSyntaxException>(() => sut.HandleCommands());
        }
    }
}