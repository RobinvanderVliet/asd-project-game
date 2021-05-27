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
