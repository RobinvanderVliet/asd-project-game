using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Models
{
    [ExcludeFromCodeCoverage]
    public class Questions
    {
        private const string EXPLORE = "Please select your explore level: 'random', 'target player' or 'target objective' type 'help explore' for more information";
        private const string COMBAT = "Please select your combat level: 'offensive' or 'defensive' or 'target objective' type 'help combat' for more information";
        private const string EXTEND_EXPLORE = "Would you like to configure custom rules for exploring?: 'yes', 'no'";
        private const string EXTEND_COMBAT = "Would you like to configure custom rules for combat?: 'yes', 'no'";

        private readonly string[] EXPLORE_ANSWERS = {"random", "target player", "target objective"};
        private readonly string[] COMBAT_ANSWERS = {"offensive", "defensive"};
        private readonly string[] EXTEND_ANSWERS = {"yes", "no"};

        public readonly string HelpCombat = "offensive = , defensive =";
        public readonly string HelpExplore = "random = ,  target player = , target objective =";

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