using System;
using System.Collections.Generic;
using System.Linq;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Items.Services;
using ASD_Game.UserInterface;

namespace ASD_Game.Session.GameConfiguration
{
    public class GameConfigurationHandler : IGameConfigurationHandler
    {
        private ConfigurationScreen _configurationScreen;
        private bool _setupConfiguration;
        private readonly IScreenHandler _screenHandler;
        private readonly IDatabaseService<GameConfigurationPOCO> _gameConfigDatabaseService;
        public IItemService ItemService { get; set; }
        public MonsterDifficulty NewMonsterDifficulty { get; private set; }
        private MonsterDifficulty CurrentMonsterDifficulty { get; }
        private ItemSpawnRate SpawnRate { get; set; }
        public string Username { get; private set; }
        private string SessionName { get; set; }
        
        public int OptionCounter { get; private set; }

        public bool NextScreen { get; private set; }

        public List<string> ConfigurationHeader { get; }

        public List<List<string>> ConfigurationChoices { get; }

        public GameConfigurationHandler(IScreenHandler screenHandler, IDatabaseService<GameConfigurationPOCO> gameConfigDatabaseService)
        {
            _setupConfiguration = false;
            _configurationScreen = screenHandler.Screen as ConfigurationScreen;
            _screenHandler = screenHandler;
            ConfigurationHeader = new List<string>();
            ConfigurationChoices = new List<List<string>>();
            NewMonsterDifficulty = MonsterDifficulty.Medium;
            CurrentMonsterDifficulty = MonsterDifficulty.Medium;
            SpawnRate = ItemSpawnRate.Medium;
            _gameConfigDatabaseService = gameConfigDatabaseService;
        }

        public void SetCurrentScreen() 
        {
            _configurationScreen = _screenHandler.Screen as ConfigurationScreen;
        }
        
        public void SetGameConfiguration()
        {
            ConfigurationChoices.Clear();
            ConfigurationHeader.Clear();
            OptionCounter = 0;
            NextScreen = false;

            //Monster difficulty
            ConfigurationHeader.Add("Choose the NPC difficulty");
            var newOptions = new List<string>() { "Easy", "Medium", "Hard", "Impossible" };
            ConfigurationChoices.Add(newOptions);

            //Item Spawn rate
            ConfigurationHeader.Add("Choose the item spawn rate");
            newOptions = new List<string>() { "Low", "Medium", "High" };
            ConfigurationChoices.Add(newOptions);

            //Username
            ConfigurationHeader.Add("Set your username");
            newOptions = new List<string>() { "We need to know what to call you", "It can be anything so please keep it civil" };
            ConfigurationChoices.Add(newOptions);

            //Session name
            ConfigurationHeader.Add("Type a session name");
            newOptions = new List<string>() { "It can be anything so please keep it civil" };
            ConfigurationChoices.Add(newOptions);
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
                        NextScreen = true;
                        break;
                    case 2:
                        NewMonsterDifficulty = MonsterDifficulty.Medium;
                        NextScreen = true;
                        break;
                    case 3:
                        NewMonsterDifficulty = MonsterDifficulty.Hard;
                        NextScreen = true;
                        break;
                    case 4:
                        NewMonsterDifficulty = MonsterDifficulty.Impossible;
                        NextScreen = true;
                        break;
                    default:
                        _configurationScreen.UpdateInputMessage(
                            "The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                        NextScreen = false;
                        break;
                }
            }
            else
            {
                _configurationScreen.UpdateInputMessage(
                    "The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                NextScreen = false;
            }

            if (NextScreen && !_configurationScreen.GetInputText().Equals("Choose an option")) 
            {
                _configurationScreen.UpdateInputMessage("Choose an option");
            }
        }

        private void UpdateItemSpawnRate(string input)
        {
            input = input.Trim('.', ' ');
            var parseSuccessful = int.TryParse(input, out var userChoice);
            if (parseSuccessful)
            {
                switch (userChoice)
                {
                    case 1:
                        SpawnRate = ItemSpawnRate.Low;
                        NextScreen = true;
                        break;
                    case 2:
                        SpawnRate = ItemSpawnRate.Medium;
                        NextScreen = true;
                        break;
                    case 3:
                        SpawnRate = ItemSpawnRate.High;
                        NextScreen = true;
                        break;
                    default:
                        _configurationScreen.UpdateInputMessage("The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                        NextScreen = false;
                        break;
                }
            }
            else 
            {
                _configurationScreen.UpdateInputMessage(
                    "The chosen option does not exist, please choose one of the existing options by typing their corresponding number");
                NextScreen = false; 
            }

            if (NextScreen && !_configurationScreen.GetInputText().Equals("Choose an option"))
            {
                _configurationScreen.UpdateInputMessage("Choose an option");
            }
        }

        public void SetDifficulty(MonsterDifficulty monsterDifficulty, string sessionId)
        {
            var gameConfiguration = _gameConfigDatabaseService.GetAllAsync().Result.FirstOrDefault(configuration => configuration.GameGUID == sessionId);

            if (gameConfiguration == null) throw new ArgumentNullException($"Configuration for game with id {sessionId} not found.");
            gameConfiguration.NPCDifficultyCurrent = gameConfiguration.NPCDifficultyNew;
            gameConfiguration.NPCDifficultyNew = (int) monsterDifficulty;
            _gameConfigDatabaseService.UpdateAsync(gameConfiguration);
        }

        public void SetSpawnRate(ItemSpawnRate spawnRate, string sessionId)
        {
            var gameConfiguration = _gameConfigDatabaseService.GetAllAsync().Result.FirstOrDefault(configuration => configuration.GameGUID == sessionId);

            if (gameConfiguration == null) throw new ArgumentNullException($"Configuration for game with id {sessionId} not found.");
            gameConfiguration.ItemSpawnRate = (int)spawnRate;
            _gameConfigDatabaseService.UpdateAsync(gameConfiguration);
            ItemService.ChanceForItemOnTile = (int)spawnRate;
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
                OptionCounter = ConfigurationChoices.Count - 2;
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
                switch (OptionCounter)
                {
                    case 0:
                        UpdateMonsterDifficulty(input);
                        break;
                    case 1:
                        UpdateItemSpawnRate(input);
                        break;
                    case 2:
                        if (input.Trim() == "")
                        {
                            _configurationScreen.UpdateInputMessage("Invalid username, please enter a valid username.");
                            NextScreen = false;
                        }
                        else
                        {
                            _configurationScreen.UpdateInputMessage("Please choose one of the options");
                            Username = input;
                            NextScreen = true;
                        }
                        
                        break;
                    case 3:
                        if (input.Trim() == "")
                        {
                            _configurationScreen.UpdateInputMessage("Invalid lobby name, please enter a valid lobby name.");
                            NextScreen = false;
                        }
                        else
                        {
                            SessionName = input;
                            NextScreen = true;
                            isCompleted = true;
                        }
                        break;
                }

                if (NextScreen)
                {
                    OptionCounter++;
                    NextScreen = false;
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
            if (OptionCounter < ConfigurationChoices.Count)
            {
                _configurationScreen?.UpdateConfigurationScreen(ConfigurationHeader[OptionCounter],
                    ConfigurationChoices[OptionCounter]);
            }
            else
            {
                _setupConfiguration = false;
            }
        }

        public ItemSpawnRate GetItemSpawnRate()
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