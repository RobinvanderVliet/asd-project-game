using NUnit.Framework;
using Agent.parser;
using static Agent.antlr.grammar.AgentConfigurationParser;
using Agent.antlr.grammar;
using Agent.antlr.ast.implementation;
using System;
using System.IO;
using Antlr4.Runtime;

namespace Agent.Tests.parser
{
    public class ASTListenerTest
    {

        private ASTAgentListener sut;

        [SetUp]
        public void Setup()
        {
            this.sut = new ASTAgentListener();
        }

        AST ParseTestFile(String resourse)
        {
            String fileContext;
            using (var sr = new StreamReader(resourse))
            {
                fileContext = sr.ReadToEnd();
            }

            ICharStream charStream = CharStreams.fromString(fileContext);
            AgentConfigurationLexer lexer = new AgentConfigurationLexer(charStream);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
        }


        [Test]
        public void EnterConfiguration1()
        {
            //Arrange
            var context = new ConfigurationContext(null, 0);

            //Act
            this.sut.EnterConfiguration(context);

            //Assert
            Assert.IsNotNull(sut.GetAST().root);
            Assert.AreEqual(new Configuration().GetNodeType(), sut.GetAST().root.GetNodeType());
        }
    }
}