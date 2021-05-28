using System;
using System.Collections.Generic;


namespace Agent.GameConfiguration
{
    public class GameConfiguration
    {
        public readonly List<string> ConfigurationHeader;
        public readonly List<List<string>> ConfigurationChoices;
        private MonsterDifficulty _newMonsterDifficulty;
        private MonsterDifficulty _currentMonsterDifficulty;

        public GameConfiguration()
        {
            ConfigurationHeader = new List<string>();
            ConfigurationChoices = new List<List<string>>();
            _newMonsterDifficulty = MonsterDifficulty.Medium;
            _currentMonsterDifficulty = MonsterDifficulty.Medium;
            SetGameConfiguration();
        }

        private void SetGameConfiguration()
        {
            //Monster difficulty
            ConfigurationHeader.Add("Welke game moeilijkheidsgraad wilt u?");
            List<string> newOptions = new List<string>() {"Easy", "Medium", "Hard", "Impossible"};
            ConfigurationChoices.Add(newOptions);
        }

        public bool AskUserForMonsterDifficulty(string input)
        {
            try
            {
                input = input.Trim('.', ' ');
                int userChoice = int.Parse(input);

                switch (userChoice)
                {
                    case 1:
                        _newMonsterDifficulty = MonsterDifficulty.Easy;
                        break;
                    case 2:
                        _newMonsterDifficulty = MonsterDifficulty.Medium;
                        break;
                    case 3:
                        _newMonsterDifficulty = MonsterDifficulty.Hard;
                        break;
                    case 4:
                        _newMonsterDifficulty = MonsterDifficulty.Impossible;
                        break;
                }
            }
            catch (Exception) 
            {
                Console.WriteLine("Aub selecteer een van de opties, nummers zijn genoeg");
                return false;
            }
            return true;
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


    }
}
