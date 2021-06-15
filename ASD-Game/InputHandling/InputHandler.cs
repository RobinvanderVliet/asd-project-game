using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ASD_Game.Agent.Services;
using ASD_Game.InputHandling.Antlr;
using ASD_Game.InputHandling.Models;
using ASD_Game.Messages;
using ASD_Game.Session;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.UserInterface;
using InputCommandHandler.Models;
using WebSocketSharp;

namespace ASD_Game.InputHandling
{
    public class InputHandler : IInputHandler
    {
        private readonly IPipeline _pipeline;
        private readonly ISessionHandler _sessionHandler;
        private readonly IScreenHandler _screenHandler;
        private readonly IMessageService _messageService;
        private readonly IGameConfigurationHandler _gameConfigurationHandler;
        private const string RETURN_KEYWORD = "return";
        private const string START_COMMAND = "start_session";

        public InputHandler(IPipeline pipeline, ISessionHandler sessionHandler, IScreenHandler screenHandler, IMessageService messageService, IGameConfigurationHandler gameConfigurationHandler)
        {
            _pipeline = pipeline;
            _sessionHandler = sessionHandler;
            _screenHandler = screenHandler;
            _gameConfigurationHandler = gameConfigurationHandler;
            _messageService = messageService;
        }

        public InputHandler()
        {
            //Empty constructor needed for testing purposes
        }

        public void HandleGameScreenCommands()
        {
            SendCommand(GetCommand());
            _screenHandler.RedrawGameInputBox();
        }

        // Needed to handle input on client when host has started session
        public void HandleGameScreenCommands(string input)
        {
            SendCommand(input);
            _screenHandler.RedrawGameInputBox();
        }

        private void SendCommand(string commando)
        {
            try
            {
                _pipeline.ParseCommand(commando);
                _pipeline.Transform();
            }
            catch (Exception e)
            {
                _messageService.AddMessage(e.Message);
            }
        }

        public virtual string GetCommand()
        {
            return _screenHandler.GetScreenInput();
        }

        public void HandleStartScreenCommands()
        {
            var input = GetCommand();
            int.TryParse(input, out var option);

            switch (option)
            {
                case 1:
                    _screenHandler.TransitionTo(new ConfigurationScreen());
                    break;
                case 2:
                    _screenHandler.TransitionTo(new SessionScreen());
                    _sessionHandler.RequestSessions();
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
                    var startScreen = _screenHandler.Screen as StartScreen;
                    startScreen.UpdateInputMessage("Not a valid option, try again!");
                    break;
            }
        }

        public void HandleSessionScreenCommands()
        {
            var sessionScreen = _screenHandler.Screen as SessionScreen;
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
                int.TryParse(input[0].ToString(), out var sessionNumber);
                var username = input[1].ToString();

                var sessionId = sessionScreen.GetSessionIdByVisualNumber(sessionNumber - 1);

                if (sessionId.IsNullOrEmpty())
                {
                    sessionScreen.UpdateInputMessage("Not a valid session, try again!");
                }
                else
                {
                    _screenHandler.TransitionTo(new LobbyScreen());
                    SendCommand("join_session \"" + sessionId + "\" \"" + inputParts[1].Replace("\"", "") + "\"");
                }
            }
        }

        public void HandleLobbyScreenCommands()
        {
            var input = GetCommand();
            // Needed to handle input on client when host has started session
            if (_screenHandler.Screen is GameScreen)
            {
                HandleGameScreenCommands(input);
            }
            else
            {
                if (input == RETURN_KEYWORD)
                {
                    _screenHandler.TransitionTo(new StartScreen());
                    return;
                }

                if (input == START_COMMAND || input.Contains("say") || input.Contains("shout"))
                {
                    SendCommand(input);
                    _screenHandler.RedrawGameInputBox();
                }
            }

        }

        public void HandleConfigurationScreenCommands()
        {
            var input = GetCommand();
            if (input == RETURN_KEYWORD)
            {
                _screenHandler.TransitionTo(new StartScreen());
                _gameConfigurationHandler.SetGameConfiguration();
            }
            else
            {
                _gameConfigurationHandler.SetCurrentScreen();
                
                var configurationCompleted = _gameConfigurationHandler.HandleAnswer(input);

                if (configurationCompleted)
                {
                    _gameConfigurationHandler.SetGameConfiguration();
                    _screenHandler.TransitionTo(new LobbyScreen());
                    _sessionHandler.CreateSession(_gameConfigurationHandler.GetSessionName(), _gameConfigurationHandler.GetUsername());
                }
            }
        }

        public void HandleEditorScreenCommands()
        {
            var questions = new Questions();
            var editorScreen = _screenHandler.Screen as EditorScreen;

            List<string> answers = new() {"explore=", "combat=", "", ""};

            var i = 0;
            Thread.Sleep(100);
            while (i < questions.EditorQuestions.Count)
            {
                editorScreen.UpdateLastQuestion(questions.EditorQuestions.ElementAt(i));

                var input = _screenHandler.GetScreenInput();
                _screenHandler.SetScreenInput(input);

                if (input.Equals("break"))
                {
                    break;
                }

                if(input.Equals("help combat"))
                {
                    _screenHandler.ConsoleHelper.ClearConsole();
                    editorScreen.UpdateLastQuestion(questions.HelpCombat);
                } else if(input.Equals("help explore"))
                {
                    _screenHandler.ConsoleHelper.ClearConsole();
                    editorScreen.UpdateLastQuestion(questions.HelpExplore);
                } else if (questions.EditorAnswers.ElementAt(i).Contains(input))
                {
                    answers[i] = answers[i] + input;
                    i++;
                    _screenHandler.ConsoleHelper.ClearConsole();
                }
                else
                {
                    editorScreen.PrintWarning("Please fill in a valid answer");
                }
            }

            if (answers.Count == questions.EditorQuestions.Count)
            {
                if (answers.ElementAt(2).Contains("yes"))
                {
                    answers.Add(CustomRuleHandleEditorScreenCommands("combat"));
                }

                if (answers.ElementAt(3).Contains("yes"))
                {
                    answers.Add(CustomRuleHandleEditorScreenCommands("explore"));
                }
            }

            var finalString = "";
            foreach (var element in answers)
            {
                if (element != "yes" && element != "no")
                {
                    finalString += element + Environment.NewLine;
                }
            }

            var agentConfigurationService = new AgentConfigurationService();
            var errors = agentConfigurationService.Configure(finalString);
            var errorsCombined = string.Empty;

            if (errors.Count != 0)
            {
                foreach (var error in errors)
                {
                    errorsCombined += error + ", ";
                }
                editorScreen.UpdateLastQuestion(errorsCombined +
                                                Environment.NewLine + "Please fix the errors and retry" +
                                                Environment.NewLine + "press enter to continue...");
                _screenHandler.GetScreenInput();
                editorScreen.ClearScreen();
                _screenHandler.TransitionTo(new StartScreen());
            }
            
            editorScreen.UpdateLastQuestion(Environment.NewLine + "Your agent has been configured successfully!" +
                                            Environment.NewLine + "press enter to continue to the start screen");
            _screenHandler.GetScreenInput();
            editorScreen.ClearScreen();
            _screenHandler.TransitionTo(new StartScreen());
        }

        public string CustomRuleHandleEditorScreenCommands(string type)
        {
            var startText = "Please enter your own " + type + " rule"
                               + Environment.NewLine
                               + "This is an example line: When agent nearby player then attack (optional: otherwise flee)"
                               + Environment.NewLine;
            var builder = new StringBuilder();
            BaseVariables variables = new();
            var nextLine = true;
            var editorScreen = _screenHandler.Screen as EditorScreen;

            string input;

            builder.Append(type + Environment.NewLine);

            editorScreen.UpdateLastQuestion("This is and example line: When agent nearby player then attack" +
                                            Environment.NewLine + "press enter to continue...");

            _screenHandler.GetScreenInput();
            editorScreen.ClearScreen();

            while (nextLine)
            {
                editorScreen.UpdateLastQuestion(
                    startText
                    + "Type Help + armor, weapon, comparison, consumables, actions, bitcoinItems, comparables for possibilities"
                    + Environment.NewLine
                    + "Type Stop to stop the custom rules editor");

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
                            editorScreen.ClearScreen();
                            editorScreen.UpdateLastQuestion("Possible armors: " + Environment.NewLine +
                                                            string.Join(", ", variables.armor) +
                                                            Environment.NewLine +
                                                            startText);
                            break;
                        case "weapon":
                            editorScreen.ClearScreen();
                            editorScreen.UpdateLastQuestion("Possible weapons: " + Environment.NewLine +
                                                            string.Join(", ", variables.weapons) +
                                                            Environment.NewLine +
                                                            startText);
                            break;
                        case "comparison":
                            editorScreen.ClearScreen();
                            editorScreen.UpdateLastQuestion("Possible comparison: " + Environment.NewLine +
                                                            string.Join(", ",
                                                                variables.comparison) +
                                                            Environment.NewLine +
                                                            startText);
                            break;
                        case "consumables":
                            editorScreen.ClearScreen();
                            editorScreen.UpdateLastQuestion("Possible consumables: " + Environment.NewLine +
                                                            string.Join(", ", variables.consumables) +
                                                            Environment.NewLine +
                                                            startText);
                            break;
                        case "actions":
                            editorScreen.ClearScreen();
                            editorScreen.UpdateLastQuestion("Possible actions: " + Environment.NewLine +
                                                            string.Join(", ", variables.actions) +
                                                            Environment.NewLine +
                                                            startText);
                            break;
                        case "bitcoinitems":
                            editorScreen.ClearScreen();
                            editorScreen.UpdateLastQuestion("Possible bitcoin items: " + Environment.NewLine +
                                                            string.Join(", ", variables.bitcoinItems) +
                                                            Environment.NewLine +
                                                            startText);
                            break;
                        case "comparables":
                            editorScreen.ClearScreen();
                            editorScreen.UpdateLastQuestion("Possible comparables: " + Environment.NewLine +
                                                            string.Join(", ", variables.comparables) +
                                                            Environment.NewLine +
                                                            startText);
                            break;
                    }

                    input = _screenHandler.GetScreenInput();
                    input = input.ToLower();
                }

                var rule = input.Split(" ").ToList();

                //Check if the user input match basic requirements
                if (CheckInput(rule, variables))
                {
                    builder.Append(input);
                }
                else
                {
                    editorScreen.ClearScreen();
                    editorScreen.UpdateLastQuestion("Your rule did not pass the check and was not added" +
                                                    Environment.NewLine + "press enter to continue...");
                    _screenHandler.GetScreenInput();
                }

                while (true)
                {
                    editorScreen.ClearScreen();
                    editorScreen.UpdateLastQuestion("Do you want to add another rule? yes or no");
                    input = _screenHandler.GetScreenInput();
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
            var correct = false;
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
            correct = rule.Intersect(variables.comparison).Any();
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
            correct = variables.comparables.Contains(rule[1]);
            if (!correct)
            {
                return correct;
            }

            //check second variable is of type item or interger
            correct = rule.IndexOf(variables.ReturnAllItems().FirstOrDefault(x => x.Equals(rule[3]))) == 3 ||
                      int.TryParse(rule[3], out _) ||
                      rule.FindLastIndex(x => x.Equals(variables.comparables.FirstOrDefault(x => x.Equals(rule[3])))) ==
                      3;
            if (!correct)
            {
                return correct;
            }

            //check is use count matches item count
            correct = rule.Count(x => x.Equals("use")) == variables.ReturnAllItems().Intersect(rule).Count();
            if (!correct)
            {
                return correct;
            }

            return correct;
        }
    }
}