using System;
using System.Collections.Generic;
using Agent.antlr.ast.implementation;
using Agent.antlr.checker;
using Agent.antlr.exception;
using Agent.antlr.grammar;
using Agent.parser;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Agent
{
    public class Pipeline
    {
        private AST ast;

        private List<string> errors;
        private Checker checker;
        // private Transformer transformer;
        // private Generator generator;

        public Pipeline()
        {
            errors = new List<string>();
            checker = new Checker();
            // transformer = new Transformer();
            // generator = new Generator();
        }

        public void ParseString(String input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            AgentConfigurationLexer lexer = new AgentConfigurationLexer(inputStream);
            lexer.RemoveErrorListeners();
            errors.Clear();
            try
            {
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                AgentConfigurationParser parser = new AgentConfigurationParser(tokens);
                parser.RemoveErrorListeners();
                var parseTree = parser.configuration();
                ParseTreeWalker walker = new ParseTreeWalker();
                
                ASTAgentListener astAgentListener = new ASTAgentListener();
                walker.Walk(astAgentListener, parseTree);
                ast = astAgentListener.GetAST();
            }
            catch (RecognitionException e)
            {
                ast = new AST();
                errors.Add(e.Message);
            }
            catch (ParseCanceledException e)
            {
                ast = new AST();
                errors.Add("Syntax error");
            }
            catch (Exception e)
            {
                errors.Add("File not found");
            }
        }

        public void CheckAst()
        {
            ThrowExpetionIfAstIsNull();
            checker.Check(ast);
        }
        
        public void TransformAst()
        {
            ThrowExpetionIfAstIsNull();
        }

        public void GenerateAst()
        {
            ThrowExpetionIfAstIsNull();
        }

        private void ThrowExpetionIfAstIsNull()
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
    }
}