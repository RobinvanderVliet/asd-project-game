using Agent.antlr.ast;
using System;
using System.Collections.Generic;
using Agent.antlr.checker;
using Agent.antlr.exception;
using Agent.antlr.grammar;
using Agent.parser;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using SyntaxErrorException = Agent.exceptions.SyntaxErrorException;

namespace Agent
{
    public class Pipeline : IAntlrErrorListener<IToken>
    {
        private AST _ast;

        private List<string> _errors;
        private Checker _checker;
        // private Transformer transformer;
        private Generator generator;

        public Pipeline()
        {
            _errors = new List<string>();
            // transformer = new Transformer();
            generator = new Generator();
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
            // checker = new Checker(ast);
            // TODO: Implement checker calls
        }
        
        public void TransformAst()
        {
            ThrowExceptionIfAstIsNull();
        }

        public string GenerateAst()
        {
            return generator.Execute(_ast);
        }

        private void ThrowExceptionIfAstIsNull()
        {
            if (_ast == null)
                throw new UndefinedAstException();
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
        
        public Checker Checker
        {
            set => _checker = value;
        }
        
        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            throw new SyntaxErrorException(msg);
        }
    }
}