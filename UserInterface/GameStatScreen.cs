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

        private String _userName = "";
        private String _score = "";
        private String _health = "";
        private String _stamina = "";
        private String _armor = "";
        private String _radiationProtectionPoints = "";
        private String _helm = "";
        private String _body = "";
        private String _weapon = "";
        private String _slotOne = "";
        private String _slotTwo = "";
        private String _slotThree = "";

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
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            ClearUserInfo();
            Console.SetCursorPosition(OFFSET_LEFT, _yPosition + 1);
            Console.Write(_userName);
            Console.SetCursorPosition(OFFSET_LEFT, _yPosition + 2);
            Console.Write("Score: " + _score);
            Console.SetCursorPosition(originalCursorX, originalCursorY);
        }
        private void DrawUserStats()
        {
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            ClearUserStats();
            int xpos = (_width / 5) + BORDER_SIZE;
            int ypos = _yPosition + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Health: " + _health);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Stamina: " + _stamina);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Armor: " + _armor);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("RPP: " + _radiationProtectionPoints);
            Console.SetCursorPosition(originalCursorX, originalCursorY);
        }
        private void DrawUserEquipment()
        {
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            ClearUserEquipment();
            int xpos = (_width / 5) * 2 + BORDER_SIZE;
            int ypos = _yPosition + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Helm: " + _helm);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Body: " + _body);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Weapon: " + _weapon);
            Console.SetCursorPosition(originalCursorX, originalCursorY);
        }
        private void DrawUserInventory()
        {
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            ClearUserInventory();
            int xpos = (_width / 5) * 3 + BORDER_SIZE;
            int ypos = _yPosition + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 1: " + _slotOne);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 2: " + _slotTwo);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 3: " + _slotThree);
            Console.SetCursorPosition(originalCursorX, originalCursorY);

        }
        private void ClearUserInfo()
        {
            int width = (_width / 5);
           
            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                Console.SetCursorPosition(OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                Console.Write(new string(' ', width));
            }

        }

        private void ClearUserStats()
        {
            int width = (_width / 5);

            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                Console.SetCursorPosition(width, _yPosition + OFFSET_TOP + i);
                Console.Write(new string(' ', width));
            }
        }

        private void ClearUserEquipment()
        {
            int width = (_width / 5);
            int xPos = (_width / 5) * 2 + BORDER_SIZE;

            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                Console.SetCursorPosition(xPos, _yPosition + OFFSET_TOP + i);
                Console.Write(new string(' ', width));
            }
        }

        private void ClearUserInventory()
        {
            int width = (_width / 5) * 2;
            int xPos = (_width / 5) * 3 + BORDER_SIZE;

            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                Console.SetCursorPosition(xPos, _yPosition + OFFSET_TOP + i);
                Console.Write(new string(' ', width));
            }
        }
        public void SetStartValues(string name, string score, string health, string stamina, string armor, string radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree)
        {
            _userName = name;
            _score = score;
            _health = health;
            _stamina = stamina;
            _armor = armor;
            _radiationProtectionPoints = radiation;
            _helm = helm;
            _body = body;
            _weapon = weapon;
            _slotOne = slotOne;
            _slotTwo = slotTwo;
            _slotThree = slotThree;
            DrawUserInfo();
            DrawUserStats();
            DrawUserEquipment();
            DrawUserInventory();
        }

        public void UpdateStat(string UpdatedStat, string newValue)
        {
            switch (UpdatedStat)
            {
                case "Name":
                    _userName = newValue;
                    DrawUserInfo();
                    break;
                case "Score":
                    _score = newValue;
                    DrawUserInfo();
                    break;
                case "Health":
                    _health = newValue;
                    DrawUserStats();
                    break;
                case "Stamina":
                    _stamina = newValue;
                    DrawUserStats();
                    break;
                case "Armor":
                    _armor = newValue;
                    DrawUserStats();
                    break;
                case "Radiation":
                    _radiationProtectionPoints = newValue;
                    DrawUserStats();
                    break;
                case "Helm":
                    _helm = newValue;
                    DrawUserEquipment();
                    break;
                case "Body":
                    _body = newValue;
                    DrawUserEquipment();
                    break;
                case "Weapon":
                    _weapon = newValue;
                    DrawUserEquipment();
                    break;
                case "SlotOne":
                    _slotOne = newValue;
                    DrawUserInventory();
                    break;
                case "SlotTwo":
                    _slotTwo = newValue;
                    DrawUserInventory();
                    break;
                case "SlotThree":
                    _slotThree = newValue;
                    DrawUserInventory();
                    break;
            }
        }
        
    }
}
