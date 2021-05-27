using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.GameConfiguration
{
    class GameConfiguration
    {
        private MonsterDifficulty _newMonsterDifficulty;
        private MonsterDifficulty _currentMonsterDifficulty;

        public GameConfiguration(MonsterDifficulty monsterDifficulty)
        {
            _newMonsterDifficulty = monsterDifficulty;
            _currentMonsterDifficulty = MonsterDifficulty.Medium;
        }

        public void AskUserForMonsterDifficulty() 
        {
            try
            {
                //Change to work with UI when implemented
                Console.WriteLine("Welke game moeilijkheidsgraad wilt u?");
                Console.WriteLine("1. Easy");
                Console.WriteLine("2. Medium");
                Console.WriteLine("3. Hard");
                Console.WriteLine("4. Impossible");
                Console.WriteLine("Voer het nummer van uw keuze in!");
                string userInput = Console.ReadLine();
                userInput = userInput.Trim(',', ' ');
                int userChoice = int.Parse(userInput);

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
                AskUserForMonsterDifficulty();
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


    }
}
