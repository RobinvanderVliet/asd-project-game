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
        private static Timer _timer;
        private const string RETURN_KEYWORD = "return";
        private string _enteredSessionName;

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
            return _screenHandler.GetScreenInput();
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
                    SendCommand("exit");
                    break;
                default:
                    StartScreen startScreen = _screenHandler.Screen as StartScreen;
                    startScreen.UpdateInputMessage("Not a valid option, try again!");
                    break;
            }
        }

        public void HandleSessionScreenCommands()
        {
            SessionScreen sessionScreen = _screenHandler.Screen as SessionScreen;
            var input = GetCommand();

            if (input == RETURN_KEYWORD)
            {
                _screenHandler.TransitionTo(new StartScreen());
                return;
            }

            var inputParts = input.Split(" ");

            if (inputParts.Length != 2)
            {
                sessionScreen.UpdateInputMessage("Provide both a session number and username (example: 1 Gerrit)");
            }
            else
            {
                int sessionNumber = 0;
                int.TryParse(input[0].ToString(), out sessionNumber);

                string sessionId = sessionScreen.GetSessionIdByVisualNumber(sessionNumber - 1);

                if (sessionId.IsNullOrEmpty())
                {
                    sessionScreen.UpdateInputMessage("Not a valid session, try again!");
                }
                else
                {
                    SendCommand("join_session \"" + sessionId + "\" \"" + inputParts[1] + "\"");
                    _screenHandler.TransitionTo(new ConfigurationScreen());
                }
            }
        }

        public void HandleEditorScreenCommands()
        {
            Questions questions = new Questions();
            EditorScreen editorScreen = _screenHandler.Screen as EditorScreen;

            List<string> answers = new();

            editorScreen.DrawHeader(questions.WELCOME);

            int i = 0;
            while (i < questions.EditorQuestions.Count)
            {
                editorScreen.UpdateLastQuestion(questions.EditorQuestions.ElementAt(i));

                var input = _screenHandler.GetScreenInput();
                _screenHandler.SetScreenInput(input);

                if (input.Equals("break"))
                {
                    break;
                }

                if (questions.EditorAnswers.ElementAt(i).Contains(input))
                {
                    answers.Add(input);
                    i++;
                    _screenHandler.ConsoleHelper.ClearConsole();
                }
                else
                {
                    editorScreen.PrintWarning("Please fill in an valid answer");
                }
            }
            
            if (answers.Count() == questions.EditorQuestions.Count)
            {
                if (answers.ElementAt(2).Contains("yes"))
                {
                    _screenHandler.SetScreenInput("BINNEN CUSTOM COMBAT RULE");
                    answers.Add(CustomRuleHandleEditorScreenCommands("combat"));
                }

                if (answers.ElementAt(3).Contains("yes"))
                {
                    _screenHandler.SetScreenInput("BINNEN CUSTOM EXPLORE RULE");
                    answers.Add(CustomRuleHandleEditorScreenCommands("explore"));
                }
            }
            
            
            
            

            //TODO: naar het volgende scherm gaan!
        }

        public string CustomRuleHandleEditorScreenCommands(string type)
        {
            StringBuilder builder = new StringBuilder();
            BaseVariables variables = new();
            bool nextLine = true;
            EditorScreen editorScreen = _screenHandler.Screen as EditorScreen;

            string input;

            builder.Append(type);

            editorScreen.UpdateLastQuestion("This is and example line: When player nearby player then attack" +
                                            Environment.NewLine + "press enter to continue...");

            _screenHandler.GetScreenInput();
            editorScreen.ClearScreen();

            while (nextLine)
            {
                editorScreen.UpdateLastQuestion(
                    "Please enter your own combat rule"
                    + Environment.NewLine
                    + "This is and example line: When player nearby player then attack (optional: otherwise flee)"
                    + Environment.NewLine
                    + "Type Help + armour, weapon, comparison, consumables, actions, bitcoinItems, comparables"
                    + "Type Stop to stop the custom rules");

                input = _screenHandler.GetScreenInput();
                input = input.ToLower();


                if (input.Equals("stop"))
                {
                    return string.Empty;
                }

                if (input.Contains("help"))
                {
                    var help = input.Split(" ");
                    switch (help[1])
                    {
                        case "armor":
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
                            editorScreen.UpdateLastQuestion(variables.actions.ToString() +
                                                            variables.actionReferences.ToString());
                            break;
                        case "bitcoinItems":
                            editorScreen.UpdateLastQuestion(variables.bitcoinItems.ToString());
                            break;
                        case "comparables":
                            editorScreen.UpdateLastQuestion(variables.comparebles.ToString());
                            break;
                    }

                    input = _screenHandler.GetScreenInput();
                    input.ToLower();
                }

                var rule = input.Split(" ").ToList();

                //basis check hier!
                if (CheckInput(rule, variables))
                {
                    builder.Append(input);
                }

                while (true)
                {
                    editorScreen.ClearScreen();
                    editorScreen.UpdateLastQuestion("Do you want to add another rule? yes or no");
                    input = GetCommand();
                    input = input.ToLower();
                    if (input.Equals("yes") || input.Equals("no"))
                    {
                        editorScreen.ClearScreen();
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

        public bool CheckInput(List<string> rule, BaseVariables variables)
        {
            bool correct = false;
            //basic rules
            //contains all two base words
            List<string> baseWords = new() {"when", "then"};
            correct = (rule.Intersect(baseWords).Count() == 2);
            if (!correct)
            {
                return correct;
            }

            //check positions of the base words
            correct = rule.IndexOf(baseWords[0]) == 0 && rule.IndexOf(baseWords[1]) == 4;
            if (!correct)
            {
                return correct;
            }

            //contains exactly 1 comparison type
            correct = (rule.Intersect(variables.comparison).Count() >= 1);
            if (!correct)
            {
                return correct;
            }

            correct = rule.IndexOf(variables.comparison.FirstOrDefault(x => x.Equals(rule[2]))) == 2;
            if (!correct)
            {
                return correct;
            }

            //check otherwise
            correct = rule.IndexOf("otherwise") == 6 || rule.IndexOf("otherwise") == 7 || rule.IndexOf("otherwise") < 0;
            if (!correct)
            {
                return correct;
            }

            //advanced rules
            correct = (rule.Intersect(variables.actionReferences).Any());
            if (correct && rule.Contains("use"))
            {
                correct = rule.Intersect(variables.ReturnAllItems()).Any();
            }

            if (!correct)
            {
                return correct;
            }

            //check first variable is of type comparebles
            correct = variables.comparebles.Contains(rule[1]);
            if (!correct)
            {
                return correct;
            }

            //check second variable is of type item or interger
            correct = rule.IndexOf(variables.ReturnAllItems().FirstOrDefault(x => x.Equals(rule[3]))) == 3 ||
                      int.TryParse(rule[3], out _) ||
                      rule.FindLastIndex(x => x.Equals(variables.comparebles.FirstOrDefault(x => x.Equals(rule[3])))) ==
                      3;
            if (!correct)
            {
                return correct;
            }

            //check is use count matches item count
            correct = rule.Where(x => x.Equals("use")).Count() == variables.ReturnAllItems().Intersect(rule).Count();
            if (!correct)
            {
                return correct;
            }

            return correct;
        }
    }
}