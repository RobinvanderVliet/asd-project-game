using NUnit.Framework;
using Agent.parser;
using static Agent.antlr.grammar.AgentConfigurationParser;
using Agent.antlr.grammar;
using Agent.antlr.ast.implementation;
using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Diagnostics;
using System.Reflection;

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
            var assembly = Assembly.GetExecutingAssembly();
            
            //using (var stream = assembly.GetManifestResourceStream("Agent.Tests.Resources.test1.txt")) 
            //Console.Write(stream.CanRead);
            using (var sr = new StreamReader( resourse)) {
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
        
        [Test]
        
        // [TestCase()]
        public void testfiletests()// (string file)
        {
            //Arrange
            var file = "test1.txt";
            var expected = Fixtures.GetFixture(file);

            //Act
            var sutt = ParseTestFile(file);

            //Assert
            Assert.AreEqual(expected,sutt);
        }
        
    }
}