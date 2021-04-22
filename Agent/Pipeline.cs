using System;
using System.Collections.Generic;
using System.Data;
using Agent.antlr.ast.implementation;
using Agent.antlr.checker;
using Agent.antlr.exception;
using Agent.antlr.grammar;
using Agent.parser;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using SyntaxErrorException = Agent.exceptions.SyntaxErrorException;

namespace Agent
{
    public class Pipeline : IAntlrErrorListener<IToken>
    {
        private AST ast;

        private List<string> errors;
        private Checker checker;
        // private Transformer transformer;
        private Generator generator;

        public Pipeline()
        {
            errors = new List<string>();
            // transformer = new Transformer();
            generator = new Generator();
        }

        public void ParseString(String input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            AgentConfigurationLexer lexer = new AgentConfigurationLexer(inputStream);
            lexer.RemoveErrorListeners();
            errors.Clear();

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AgentConfigurationParser parser = new AgentConfigurationParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(this);
            var parseTree = parser.configuration();
            ParseTreeWalker walker = new ParseTreeWalker();
            
            ASTAgentListener astAgentListener = new ASTAgentListener();
            walker.Walk(astAgentListener, parseTree);
            ast = astAgentListener.GetAST();
        }

        public void CheckAst()
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
            return generator.execute(ast);
        }

        private void ThrowExceptionIfAstIsNull()
        {
            if (ast == null)
                throw new UndefinedAstException();
        }

        public AST Ast
        {
            get => ast;
            set => ast = value;
        }

        public List<string> Errors => errors;

        public void ClearErrors()
        {
            errors.Clear();
        }
        
        public Checker Checker
        {
            set => checker = value;
        }
        
        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            throw new SyntaxErrorException(msg);
        }
    }
}