using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using InputHandling.Antlr;
using InputHandling.Exceptions;
using WebSocketSharp;
using InputCommandHandler.Models;
using Session;
using UserInterface;
using System.Text;
using InputHandling.Models;

namespace InputHandling
{
    public class InputHandler : IInputHandler
    {
        private IPipeline _pipeline;
        private ISessionHandler _sessionHandler;
        private IScreenHandler _screenHandler;
        private static Timer  _timer;
        private const string RETURN_KEYWORD = "return";

        public InputHandler(IPipeline pipeline, ISessionHandler sessionHandler, IScreenHandler screenHandler)
        {
            _pipeline = pipeline;
            _sessionHandler = sessionHandler;
            _screenHandler = screenHandler;
        }

        public InputHandler()
        {

        }

        public void HandleGameScreenCommands()
        {
            SendCommand(GetCommand());
        }
        private void SendCommand(string commando)
        {
            try
            {
                _pipeline.ParseCommand(commando);
                _pipeline.Transform();
            }
            catch (CommandSyntaxException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (MoveException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public virtual string GetCommand()
        {
            return Console.ReadLine();
        }
        
        public void HandleStartScreenCommands()
        {
            var input = GetCommand();
            var option = 0;
            int.TryParse(input, out option);
            
            switch (option)
            {
                case 1:
                    _screenHandler.TransitionTo(new ConfigurationScreen());
                    break;
                case 2:
                    _sessionHandler.RequestSessions();
                    _screenHandler.TransitionTo(new SessionScreen());
                    break;
                case 3:
                    _screenHandler.TransitionTo(new LoadScreen());
                    break;
                case 4:
                    _screenHandler.TransitionTo(new EditorScreen());
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    StartScreen startScreen = _screenHandler.Screen as StartScreen;
                    startScreen.UpdateInputMessage("Not a valid option, try again!");
                    break;
            }
        }

        public void HandleSessionScreenCommands()
        {
            var input = GetCommand();
            int sessionNumber = 0;
            int.TryParse(input, out sessionNumber);
            SessionScreen sessionScreen = _screenHandler.Screen as SessionScreen;

            if (input == RETURN_KEYWORD)
            {
                _screenHandler.TransitionTo(new StartScreen());
            }
            else if (sessionNumber > 0)
            {
                string sessionId = sessionScreen.GetSessionIdByVisualNumber(sessionNumber - 1);

                if (sessionId.IsNullOrEmpty())
                {
                    sessionScreen.UpdateInputMessage("Not a valid session, try again!");
                }
                else
                {
                    SendCommand("join_session \"" + sessionId + "\"");
                    _screenHandler.TransitionTo(new ConfigurationScreen()); // maybe a waiting screen in stead?   
                }
            }
            else
            {
                sessionScreen.UpdateInputMessage("That is not a number, please try again!");
            }
        }

        public void HandleEditorScreenCommands()
        {
            // de huidige vraag tonen
            Questions questions = new Questions();
            EditorScreen editorScreen = _screenHandler.Screen as EditorScreen;

            List<string> answers = new();

            editorScreen.DrawHeader(questions.WELCOME);
            
            int i = 0;
            while (i < questions.EditorQuestions.Count)
            {
                editorScreen.UpdateLastQuestion(questions.EditorQuestions.ElementAt(i));

                var input = Console.ReadLine();
                Console.WriteLine(input);

                if (input.Equals("break"))
                {
                    break;
                }

                if (questions.EditorAnswers.ElementAt(i).Contains(input))
                {
                    answers.Add(input);
                    i++;
                    Console.Clear();
                }
                else
                {
                    editorScreen.PrintWarning("Please fill in an valid answer");
                } 
            }

            if (answers.ElementAt(2).Contains("yes"))
            {
                Console.WriteLine("BINNEN CUSTOM COMBAT RULE");
                answers.Add(CustomRuleHandleEditorScreenCommands("combat"));
            }

            if (answers.ElementAt(3).Contains("yes"))
            {
                Console.WriteLine("BINNEN CUSTOM EXPLORE RULE");
                answers.Add(CustomRuleHandleEditorScreenCommands("explore"));
            }

            //naar de volgende scherm gaan!
        }

        private string CustomRuleHandleEditorScreenCommands(string type)
        {
            StringBuilder builder = new StringBuilder();
            BaseVariables variables = new();
            bool nextLine = true;
            EditorScreen editorScreen = _screenHandler.Screen as EditorScreen;

            string input;

            builder.Append(type);

            editorScreen.UpdateLastQuestion("This is and example line: When player nearby player then attack" + Environment.NewLine + "press enter to continue...");
            
            Console.ReadLine();
            Console.Clear();

            while (nextLine)
            {
                editorScreen.UpdateLastQuestion(
                    "Please enter your own combat rule" 
                    + Environment.NewLine
                    + "This is and example line: When player nearby player then attack (optional: otherwise flee)"
                    + Environment.NewLine
                    + "Type Help + armour, weapon, comparison, consumables, actions, bitcoinItems, comparables"
                    + "Type Stop to stop the custom rules" );

                input = Console.ReadLine();
                input = input.ToLower();


                if (input.Equals("Stop"))
                {
                    return string.Empty;
                }

                if (input.Contains("help"))
                {
                    var help = input.Split(" ");
                    switch (help[1])
                    {
                        case "armour":
                            editorScreen.UpdateLastQuestion(variables.armor.ToString());
                            break;
                        case "weapon":
                            editorScreen.UpdateLastQuestion(variables.weapons.ToString());
                            break;
                        case "comparison":
                            editorScreen.UpdateLastQuestion(variables.comparison.ToString());
                            break;
                        case "consumables":
                            editorScreen.UpdateLastQuestion(variables.consumables.ToString());
                            break;
                        case "actions":
                            editorScreen.UpdateLastQuestion(variables.actions.ToString() + variables.actionReferences.ToString());
                            break;
                        case "bitcoinItems":
                            editorScreen.UpdateLastQuestion(variables.bitcoinItems.ToString());
                            break;
                        case "comparables":
                            editorScreen.UpdateLastQuestion(variables.comparebles.ToString());
                            break;
                    }
                }

                input = Console.ReadLine();
                input = input.ToLower();
                var rule = input.Split(" ");

                //basis check hier!
                if (CheckInput(rule, variables))
                {
                    builder.Append(rule.ToString());
                }

                while (true) { 
                    Console.Clear();
                    editorScreen.UpdateLastQuestion("Do you want to add another rule? yes or no");
                    input = Console.ReadLine();
                    input = input.ToLower();
                    if(input.Equals("yes") || input.Equals("no"))
                    {
                        break;
                    }
                }

                if (input.Equals("no"))
                {
                    nextLine = false;
                }
            }

            return builder.ToString();
        }

        private bool CheckInput(string[] rule, BaseVariables variables)
        {
            bool correct = false;
            for (int j = 0; j < rule.Length; j++)
            {
                switch (j)
                {
                    case 0: //when
                        correct = rule[j].Equals("when");
                        break;
                    case 1: //left comparable
                        correct = variables.comparebles.Contains(rule[j]);
                        break;
                    case 2: //comparison
                        correct = variables.comparison.Contains(rule[j]);
                        break;
                    case 3: //right comparable
                        if (variables.comparebles.Contains(rule[j]) ||
                            variables.ReturnAllItems().Contains(rule[j]) ||
                            int.TryParse(rule[j], out _))
                        {
                            correct = true;
                        }
                        break;
                    case 4: //then
                        correct = rule[j].Equals("then");
                        break;
                    case 5: //action
                            correct = variables.actions.Contains(rule[j]);
                        break;
                    case 6:
                        //check use
                        if (variables.ReturnAllItems().Contains(rule[j]))
                        {
                            correct = true;
                        } else
                        {//otherwise
                            correct = rule[j].Contains("otherwise");
                        }
                        break;
                    case 7:
                        //check otherwise
                        if (rule[j].Contains("otherwise"))
                        {
                            correct = true;
                        }
                        else
                        {//action
                            if(variables.actions.Contains(rule[j])|| variables.actionReferences.Contains(rule[j]))
                            {
                                correct = true;
                            }
                        }
                        break;
                    case 8://action
                        correct = variables.ReturnAllItems().Contains(rule[j]);
                        break;
                }
                if (!correct)
                {
                    break;
                }
            }
            return correct;
        }
    }
}