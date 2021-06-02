using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{

    class GameStatScreen : Screen
    {
        private const int STAT_X = 0;
        private const int STAT_Y = 0;
        private const int STAT_WIDTH = SCREEN_WIDTH - 2;
        private int _statHeight;

        private string _userName = "TEMP USERNAME";
        private int _score = 0;
        private int _hp = 100;
        private int _stamina = 100;
        private int _armor = 100;
        private int _radiationProtectionPoints = 100;
        private string _helm = "Bandana";
        private string _body = "Jacket";
        private string _melee = "Knife";
        private string _ranged = "AK-47";
        private string _slotOne = "Bandage";
        private string _slotTwo = "Suspicious white powder";
        private string _slotThree = "Medkit";

        public GameStatScreen(int height)
        {
            _statHeight = height;
        }
        public override void DrawScreen()
        {
            DrawStatBox();            
        }

        public void DrawStatBox()
        {
            DrawBox(STAT_X, STAT_Y, STAT_WIDTH, _statHeight);
            DrawUserInfo();
            DrawUserStats();
            DrawUserEquipment();
            DrawUserInventory();
        }
        private void DrawUserInfo()
        {
            Console.SetCursorPosition(2, STAT_Y + 1);
            Console.Write(_userName);
            Console.SetCursorPosition(2, STAT_Y + 2);
            Console.Write("Score: " + _score);
        }
        private void DrawUserStats()
        {
            int xpos = (STAT_WIDTH / 5) + 2;
            int ypos = STAT_Y + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("HP: " + _hp);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Stamina: " + _stamina);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Armor: " + _armor);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("RPP: " + _radiationProtectionPoints);
        }
        private void DrawUserEquipment()
        {
            int xpos = (STAT_WIDTH / 5) * 2 + 2;
            int ypos = STAT_Y + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Helm: " + _helm);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Body: " + _body);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Melee: " + _melee);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Ranged: " + _ranged);
        }
        private void DrawUserInventory()
        {
            int xpos = (STAT_WIDTH / 5) * 3 + 2;
            int ypos = STAT_Y + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 1: " + _slotOne);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 2: " + _slotTwo);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 3: " + _slotThree);

        }

        public int getStatHeight()
        {
            return _statHeight;
        }
    }
}
