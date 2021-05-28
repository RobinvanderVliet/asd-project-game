using System;
using System.Linq;
using InputCommandHandler;

namespace Agent
{
    public class ConfigEditor
    {
        private InputCommandHandlerComponent _inputCommandHandler;

        private const string WELCOME = "Welcome to the ingame agent configurator!";
        private const string EXPLORE = "Please select your explore level: 'random', 'target player' or 'target objective'";
        private string[] EXPLORE_RESPONSE = {"random", "target player", "target objective"};
        private const string COMBAT = "Please select your combat level: 'offensive', 'defensive' or 'flee'";
        private const string EXTEND_EXPLORE = "Would you like to configure custom rules for exploring?: 'explore yes', 'explore no'";
        private const string EXTEND_COMBAT = "Would you like to configure custom rules for combat?: 'combat yes', 'combat no'";

        

        public ConfigEditor(InputCommandHandlerComponent inputCommandHandlerComponent)
        {
            _inputCommandHandler = inputCommandHandlerComponent;
        }

        public void startEditor()
        {
            //EXPLORE
            string configString = string.Empty;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(WELCOME);
            Console.WriteLine(EXPLORE);
            if (EXPLORE_RESPONSE.Contains(_inputCommandHandler.GetCommand()))
            {
                Console.WriteLine("MARK HOUDT VAN PIE????");
                var exploreLevel = _inputCommandHandler.GetCommand();
                configString += "explore=" + exploreLevel + Environment.NewLine;
                Console.WriteLine(EXTEND_EXPLORE);
                if (configString != string.Empty && _inputCommandHandler.GetCommand() == "explore yes")
                {
                    Console.WriteLine("YOU HAVE CHOSEN FOR CUSTOM EXPLORE RULES.......");
                
                }
            
                //COMBAT
                Console.WriteLine(COMBAT);
                var combatLevel = _inputCommandHandler.GetCommand();
                configString += "combat=" + exploreLevel + Environment.NewLine;
                Console.WriteLine(EXTEND_COMBAT);
                if (_inputCommandHandler.GetCommand() == "combat yes")
                {
                    Console.WriteLine("YOU HAVE CHOSEN FOR CUSTOM COMBAT RULES.......");
                }
            }
            else
            {
                Console.WriteLine("ERROR! You need to choose a valid option!");
                startEditor();
            }
            
            
            
        }
    }
}