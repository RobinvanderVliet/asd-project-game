using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{

    class GameStatScreen : Screen
    {
        private int _xPosition;
        private int _yPosition;
        private int _width;
        private int _height;

        private String _userName = "TEMP USERNAME";
        private int _score = 0;
        private int _hp = 100;
        private int _stamina = 100;
        private int _armor = 100;
        private int _radiationProtectionPoints = 100;
        private String _helm = "Bandana";
        private String _body = "Jacket";
        private String _melee = "Knife";
        private String _ranged = "AK-47";
        private String _slotOne = "Bandage";
        private String _slotTwo = "Suspicious white powder";
        private String _slotThree = "Medkit";

        public GameStatScreen(int x, int y, int width, int height)
        {
            _xPosition = x;
            _yPosition = y;
            _width = width;
            _height = height;
        }
        public override void DrawScreen()
        {
            DrawStatBox();            
        }

        public void DrawStatBox()
        {
            DrawBox(_xPosition, _yPosition, _width, _height);
            DrawUserInfo();
            DrawUserStats();
            DrawUserEquipment();
            DrawUserInventory();
        }
        private void DrawUserInfo()
        {
            Console.SetCursorPosition(2, _yPosition + 1);
            Console.Write(_userName);
            Console.SetCursorPosition(2, _yPosition + 2);
            Console.Write("Score: " + _score);
        }
        private void DrawUserStats()
        {
            int xpos = (_width / 5) + BORDER_SIZE;
            int ypos = _yPosition + 1;
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
            int xpos = (_width / 5) * 2 + BORDER_SIZE;
            int ypos = _yPosition + 1;
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
            int xpos = (_width / 5) * 3 + BORDER_SIZE;
            int ypos = _yPosition + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 1: " + _slotOne);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 2: " + _slotTwo);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 3: " + _slotThree);

        }

        public int getStatHeight()
        {
            return _height;
        }
    }
}
