using System;
using System.Collections.Generic;
using System.Linq;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.UserInterface;

namespace ASD_Game.Session.GameConfiguration
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
        private IDatabaseService<GameConfigurationPOCO> _gameConfigDatabaseService;
        public MonsterDifficulty NewMonsterDifficulty { get; set; }
        public MonsterDifficulty CurrentMonsterDifficulty { get; set; }
        public ItemSpawnRate SpawnRate { get; set; }
        public string Username { get; set; }
        public string SessionName { get; set; }

        public GameConfigurationHandler(IScreenHandler screenHandler, IDatabaseService<GameConfigurationPOCO> gameConfigDatabaseService)
        {
            _setupConfiguration = false;
            _configurationScreen = screenHandler.Screen as ConfigurationScreen;
            _screenHandler = screenHandler;
            _configurationHeader = new List<string>();
            _configurationChoices = new List<List<string>>();
            NewMonsterDifficulty = MonsterDifficulty.Medium;
            CurrentMonsterDifficulty = MonsterDifficulty.Medium;
            SpawnRate = ItemSpawnRate.Low;
            _gameConfigDatabaseService = gameConfigDatabaseService;
        }

        public void SetGameConfiguration()
        {
            _configurationChoices.Clear();
            _configurationHeader.Clear();
            _optionCounter = 0;
            _nextScreen = false;

            List<string> newOptions = new List<string>();

            //Monster difficulty
            _configurationHeader.Add("Choose the NPC difficulty");
            newOptions = new List<string>() { "Easy", "Medium", "Hard", "Impossible" };
            _configurationChoices.Add(newOptions);

            //Item Spawnrate
            _configurationHeader.Add("Choose the item spawnrate");
            newOptions = new List<string>() { "Low", "Medium", "High" };
            _configurationChoices.Add(newOptions);

            //Username
            _configurationHeader.Add("Set your username");
            newOptions = new List<string>() { "We need to know what to call you", "It can be anything so please keep it civil" };
            _configurationChoices.Add(newOptions);

            //Session name
            _configurationHeader.Add("Type a session name");
            newOptions = new List<string>() { "It can be anything so please keep it civil" };
            _configurationChoices.Add(newOptions);
        }

        public void UpdateMonsterDifficulty(string input)
        {
            input = input.Trim('.', ' ');
            bool parseSuccessful = int.TryParse(input, out int userChoice);
            if (parseSuccessful)
            {
                switch (userChoice)
                {
                    case 1:
                        NewMonsterDifficulty = MonsterDifficulty.Easy;
                        _nextScreen = true;
                        break;
                    case 2:
                        NewMonsterDifficulty = MonsterDifficulty.Medium;
                        _nextScreen = true;
                        break;
                    case 3:
                        NewMonsterDifficulty = MonsterDifficulty.Hard;
                        _nextScreen = true;
                        break;
                    case 4:
                        NewMonsterDifficulty = MonsterDifficulty.Impossible;
                        _nextScreen = true;
                        break;
                    default:
                        _configurationScreen.UpdateInputMessage(
                            "The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                        _nextScreen = false;
                        break;
                }
            }
            else
            {
                _configurationScreen.UpdateInputMessage(
                    "The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                _nextScreen = false;
            }
        }

        public void UpdateItemSpawnrate(string input)
        {
            input = input.Trim('.', ' ');
            bool parseSuccessful = int.TryParse(input, out int userChoice);
            if (parseSuccessful)
            {
                switch (userChoice)
                {
                    case 1:
                        SpawnRate = ItemSpawnRate.Low;
                        _nextScreen = true;
                        break;
                    case 2:
                        SpawnRate = ItemSpawnRate.Medium;
                        _nextScreen = true;
                        break;
                    case 3:
                        SpawnRate = ItemSpawnRate.High;
                        _nextScreen = true;
                        break;
                    default:
                        _configurationScreen.UpdateInputMessage("The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                        _nextScreen = false;
                        break;
                }
            }
            else 
            {
                _configurationScreen.UpdateInputMessage(
                    "The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                _nextScreen = false; 
            }
        }

        public void SetDifficulty(MonsterDifficulty monsterDifficulty, string sessionId)
        {
            var gameConfiguration = _gameConfigDatabaseService.GetAllAsync().Result.FirstOrDefault(configuration => configuration.GameGUID == sessionId);

            gameConfiguration.NPCDifficultyCurrent = gameConfiguration.NPCDifficultyNew;
            gameConfiguration.NPCDifficultyNew = (int)monsterDifficulty;
            _gameConfigDatabaseService.UpdateAsync(gameConfiguration);
        }

        public void SetSpawnRate(ItemSpawnRate spawnRate, string sessionId)
        {
            var gameConfiguration = _gameConfigDatabaseService.GetAllAsync().Result.FirstOrDefault(configuration => configuration.GameGUID == sessionId);

            gameConfiguration.ItemSpawnRate = (int) spawnRate;
            _gameConfigDatabaseService.UpdateAsync(gameConfiguration);
        }

        public int AdjustMonsterValue(int monsterProperty)
        {
            return monsterProperty / (int)CurrentMonsterDifficulty * (int)NewMonsterDifficulty;
        }

        private void CheckSetupConfiguration(String input)
        {
            if (input.Equals("1"))
            {
                _setupConfiguration = true;
                SetGameConfiguration();
                UpdateScreen();
            }
            else if (input.Equals("2"))
            {
                _setupConfiguration = true;
                SetGameConfiguration();
                _optionCounter = _configurationChoices.Count - 2;
                NewMonsterDifficulty = MonsterDifficulty.Medium;
                SpawnRate = ItemSpawnRate.Medium;
                UpdateScreen();
            }
            else
            {
                _configurationScreen.UpdateInputMessage("Please choose one of the options");
            }
        }

        public bool HandleAnswer(string input)
        {
            bool isCompleted = false;
            if (_setupConfiguration)
            {
                switch (_optionCounter)
                {
                    case 0:
                        UpdateMonsterDifficulty(input);
                        break;
                    case 1:
                        UpdateItemSpawnrate(input);
                        break;
                    case 2:
                        Username = input;
                        _nextScreen = true;
                        break;
                    case 3:
                        SessionName = input;
                        _nextScreen = true;
                        isCompleted = true;
                        break;
                }

                if (_nextScreen)
                {
                    _optionCounter++;
                    _nextScreen = false;
                }
                UpdateScreen();
            }
            else if (!_setupConfiguration)
            {
                CheckSetupConfiguration(input);
            }
            return isCompleted;
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
                _setupConfiguration = false;
            }
        }

        public int OptionCounter
        {
            get { return _optionCounter; }
        }

        public bool NextScreen
        {
            get { return _nextScreen; }
        }

        public List<string> ConfigurationHeader
        {
            get { return _configurationHeader; }
        }

        public List<List<string>> ConfigurationChoices
        {
            get { return _configurationChoices; }
        }

        public ItemSpawnRate GetSpawnRate()
        {
            return SpawnRate;
        }

        public MonsterDifficulty GetCurrentMonsterDifficulty()
        {
            return CurrentMonsterDifficulty;
        }

        public MonsterDifficulty GetNewMonsterDifficulty()
        {
            return NewMonsterDifficulty;
        }

        public string GetUsername()
        {
            return Username;
        }

        public string GetSessionName()
        {
            return SessionName;
        }
    }
}