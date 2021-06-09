using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Agent.Antlr.Grammar;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ASD_Game.Agent.Antlr.Ast;
using ASD_Game.Agent.Antlr.Parser;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Parser
{
    [ExcludeFromCodeCoverage]
    public class ASTListenerTest
    {

        AST ParseTestFile(String resourse)
        {

            String fileContext;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith(resourse));

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var sr = new StreamReader(stream))
            {
                fileContext = sr.ReadToEnd();
            }


            AntlrInputStream charStream = new AntlrInputStream(fileContext);
            AgentConfigurationLexer lexer = new AgentConfigurationLexer(charStream);

            CommonTokenStream tokens = new CommonTokenStream(lexer);

            AgentConfigurationParser parser = new AgentConfigurationParser(tokens);
            parser.ErrorHandler = new BailErrorStrategy();

            var errorListener = new TestErrorHandler();

            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);

            ASTAgentListener listener = new ASTAgentListener();

            try
            {
                IParseTree parseTree = parser.configuration();
                ParseTreeWalker walker = new ParseTreeWalker();
                walker.Walk(listener, parseTree);
            }
            catch (Exception e)
            {
                Assert.Fail(errorListener.ToString());
            }

            return listener.GetAST();

        }

        [Test]
        [TestCase("test1.txt")]
        [TestCase("test2.txt")]
        [TestCase("test3.txt")]
        [Ignore("Voor deze testen gaat er iets mis met NUNIT voor test 2 en 3")]
        public void Test_FileTests_MultipleFiles(String file)
        {
            //Arrange
            var expected = Fixtures.GetFixture(file);

            //Act
            var sut = ParseTestFile(file);

            //Assert
            Assert.AreEqual(expected.Root.ToString(), sut.Root.ToString());
        }

    }
}