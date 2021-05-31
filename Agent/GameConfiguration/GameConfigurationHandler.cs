using System;
using System.Collections.Generic;
using UserInterface;

namespace Agent.GameConfiguration
{
    public class GameConfigurationHandler : IGameConfigurationHandler
    {
        private ConfigurationScreen _configurationScreen;
        private int _optionCounter;
        private bool _nextScreen;
        private bool _setupConfiguration;
        private readonly IScreenHandler _screenHandler;
        private readonly List<string> _configurationHeader;
        private readonly List<List<string>> _configurationChoices;
        private MonsterDifficulty _newMonsterDifficulty;
        private MonsterDifficulty _currentMonsterDifficulty;

        public GameConfigurationHandler(IScreenHandler screenHandler)
        {
            _setupConfiguration = false;
            _configurationScreen = screenHandler.Screen as ConfigurationScreen;
            _screenHandler = screenHandler;
            _configurationHeader = new List<string>();
            _configurationChoices = new List<List<string>>();
            _newMonsterDifficulty = MonsterDifficulty.Medium;
            _currentMonsterDifficulty = MonsterDifficulty.Medium;
        }

        public void SetGameConfiguration()
        {
            _optionCounter = 0;
            _nextScreen = false;

            List<string> newOptions = new List<string>();
            
            //Monster difficulty
            _configurationHeader.Add("Difficulty");
            newOptions = new List<string>() {"Easy", "Medium", "Hard", "Impossible"};
            _configurationChoices.Add(newOptions);
        }

        public void UpdateMonsterDifficulty(string input)
        {
            try
            {
                input = input.Trim('.', ' ');
                int userChoice = int.Parse(input);

                switch (userChoice)
                {
                    case 1:
                        _newMonsterDifficulty = MonsterDifficulty.Easy;
                        _nextScreen = true;
                        break;
                    case 2:
                        _newMonsterDifficulty = MonsterDifficulty.Medium;
                        _nextScreen = true;
                        break;
                    case 3:
                        _newMonsterDifficulty = MonsterDifficulty.Hard;
                        _nextScreen = true;
                        break;
                    case 4:
                        _newMonsterDifficulty = MonsterDifficulty.Impossible;
                        _nextScreen = true;
                        break;
                    default:
                        _configurationScreen.UpdateInputMessage("The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                        _nextScreen = false;
                        break;
                }
            }
            catch (Exception) 
            {
                _configurationScreen.UpdateInputMessage("The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                _nextScreen = false;
            }
        }

        public void SetDifficulty(MonsterDifficulty monsterDifficulty) 
        {
            _currentMonsterDifficulty = _newMonsterDifficulty;
            _newMonsterDifficulty = monsterDifficulty;
        }

        //Moet aangeroepen worden tijdens creatie monsters en bij aanpas command
        public int AdjustMonsterValue(int monsterProperty) 
        {
            return monsterProperty / (int)_currentMonsterDifficulty * (int)_newMonsterDifficulty;
            
        }

        private void CheckSetupConfiguration(String input)
        {
            if (input.Equals("1"))
            {
                _setupConfiguration = true;
                SetGameConfiguration();
                UpdateScreen();
            } else if (input.Equals("2"))
            {
                _setupConfiguration = false;
                //Set default config and move on
            }
            else
            {
                _configurationScreen.UpdateInputMessage("Please choose one of the options");
            }
        }

        public void HandleAnswer(string input)
        {
            if (_setupConfiguration)
            {
                switch (_optionCounter)
                {
                    case 0:
                        UpdateMonsterDifficulty(input);
                        break;
                }
            
                if (_nextScreen)
                {
                    _optionCounter++;
                    _nextScreen = false;
                }
                UpdateScreen();
            } else if (!_setupConfiguration)
            {
                CheckSetupConfiguration(input);
            }
        }

        private void UpdateScreen()
        {
            _configurationScreen = _screenHandler.Screen as ConfigurationScreen;
            if (_optionCounter < _configurationChoices.Count)
            {
                _configurationScreen.UpdateConfigurationScreen(_configurationHeader[_optionCounter],
                    _configurationChoices[_optionCounter]);
            }
            else
            {
                _configurationScreen.UpdateInputMessage("That's all folks!");
                _setupConfiguration = false;
                SetGameConfiguration();
                //To the next screen!
            }
        }
    }
}