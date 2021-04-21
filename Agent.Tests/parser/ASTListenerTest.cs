using NUnit.Framework;
using Agent.parser;
using static Agent.antlr.grammar.AgentConfigurationParser;
using Agent.antlr.grammar;
using Agent.antlr.ast.implementation;
using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Reflection;
using System.Linq;

namespace Agent.Tests.parser
{
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

            try {
                IParseTree parseTree = parser.configuration();
                ParseTreeWalker walker = new ParseTreeWalker();
                walker.Walk(listener,parseTree);
            }
            catch (Exception e) {
                Assert.Fail(errorListener.ToString());
            }
            
            return listener.GetAST();
    
        }
        
        [Test]
        [TestCase("test1.txt")]
        [TestCase("test2.txt")]
        [TestCase("test3.txt")]
        public void TestFileTests1(String file)
        {
            //Arrange
            var expected = Fixtures.GetFixture(file);

            //Act
            var sut = ParseTestFile(file);

            //Assert
            Assert.AreEqual(expected.ToString(), sut.ToString());
        }

    }
}