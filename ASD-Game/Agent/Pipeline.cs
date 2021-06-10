using System;
using System.Collections.Generic;
using Agent.Antlr.Grammar;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ASD_Game.Agent.Antlr.Ast;
using ASD_Game.Agent.Antlr.Checker;
using ASD_Game.Agent.Antlr.Parser;
using ASD_Game.Agent.Generator;
using SyntaxErrorException = ASD_Game.Agent.Exceptions.SyntaxErrorException;
using ASD_Game.Messages;
using ASD_Game.UserInterface;

namespace ASD_Game.Agent
{
    public class Pipeline : IAntlrErrorListener<IToken>
    {
        private AST _ast;

        private List<string> _errors;
        private Checking _checking;
        private Generating _generating;
        private MessageService _messageService;

        public Pipeline()
        {
            _errors = new List<string>();
            _generating = new Generating();
            _checking = new Checking();
            _messageService = new(new ScreenHandler());
        }

        public virtual void ParseString(String input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            AgentConfigurationLexer lexer = new AgentConfigurationLexer(inputStream);
            lexer.RemoveErrorListeners();
            _errors.Clear();

            try
            {
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
            catch (Exception e)
            {
                _messageService.AddMessage("Syntax error: " + e.Message);
                _errors.Add(e.Message);
            }
        }

        public virtual void CheckAst()
        {
            _checking.Check(_ast.Root);
            foreach (var error in _ast.GetErrors())
            {
                _messageService.AddMessage("Semantic error: " + error.ToString());
                _errors.Add(error.ToString());
            }
        }

        public virtual string GenerateAst()
        {
            return _generating.Execute(_ast);
        }

        public AST Ast
        {
            get => _ast;
            set => _ast = value;
        }

        public List<string> Errors
        {
            get => _errors;
        }
        public virtual List<string> GetErrors()
        {
            return _errors;
        }

        public void ClearErrors()
        {
            _errors.Clear();
        }

        public Checking Checking
        {
            get => _checking;
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