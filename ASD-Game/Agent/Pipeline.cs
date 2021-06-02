using Agent.Antlr.Ast;
using System;
using System.Collections.Generic;
using Agent.Antlr.Checker;
using Agent.Antlr.Grammar;
using Agent.Antlr.Parser;
using Agent.Generator;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using SyntaxErrorException = Agent.Exceptions.SyntaxErrorException;

namespace Agent
{
    public class Pipeline : IAntlrErrorListener<IToken>
    {
        private AST _ast;

        private List<string> _errors;
        private Checking _checking;
        private Generating _generating;

        public Pipeline()
        {
            _errors = new List<string>();
            // transformer = new Transformer();
            _generating = new Generating();
        }

        public void ParseString(String input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            AgentConfigurationLexer lexer = new AgentConfigurationLexer(inputStream);
            lexer.RemoveErrorListeners();
            _errors.Clear();

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AgentConfigurationParser parser = new AgentConfigurationParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(this);
            var parseTree = parser.configuration();
            ParseTreeWalker walker = new ParseTreeWalker();
            
            ASTAgentListener astAgentListener = new ASTAgentListener();
            walker.Walk(astAgentListener, parseTree);
            _ast = astAgentListener.GetAST();

        }

        public virtual void CheckAst()
        {
            if(_checking == null)
            {
                _checking = new Checking(_ast);
            }
            _checking.Check(_ast);
        }

        public string GenerateAst()
        {
            return _generating.Execute(_ast);
        }

        public AST Ast
        {
            get => _ast;
            set => _ast = value;
        }

        public List<string> Errors => _errors;

        public void ClearErrors()
        {
            _errors.Clear();
        }
        
        public Checking Checking
        {
            set => _checking = value;
        }
        
        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            throw new SyntaxErrorException(msg);
        }
    }
}