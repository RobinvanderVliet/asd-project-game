using System;
using System.Collections.Generic;
using System.Xml;

namespace InputCommandHandler.Models
{
    public class Questions
    {
        public readonly string WELCOME = "Welcome to the ingame agent configurator!";
        private const string EXPLORE = "Please select your explore level: 'random', 'target player' or 'target objective'";
        private const string COMBAT = "Please select your combat level: 'offensive', 'defensive' or 'flee'";
        private const string EXTEND_EXPLORE = "Would you like to configure custom rules for exploring?: 'yes', 'no'";
        private const string EXTEND_COMBAT = "Would you like to configure custom rules for combat?: 'yes', 'no'";

        private readonly string[] EXPLORE_ANSWERS = {"random", "target player", "target objective"};
        private readonly string[] COMBAT_ANSWERS = {"offensive", "defensive", "flee"};
        private readonly string[] EXTEND_ANSWERS = {"yes", "no"};

        private List<string> _editorQuestions;
        private List<string[]> _editorAnswers;
    
        public List<string> EditorQuestions
        {
            get => _editorQuestions;
            set => _editorQuestions = value;
        }
        
        public List<string[]> EditorAnswers
        {
            get => _editorAnswers;
            set => _editorAnswers = value;
        }

        public Questions()
        {
            _editorQuestions = new (){EXPLORE, COMBAT, EXTEND_EXPLORE, EXTEND_COMBAT};
            _editorAnswers = new (){EXPLORE_ANSWERS, COMBAT_ANSWERS, EXTEND_ANSWERS, EXTEND_ANSWERS};

            if (_editorAnswers.Count != _editorQuestions.Count)
            {
                throw new Exception();
            }
        }
        
    }
}