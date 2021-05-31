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

        public GameStatScreen(int x, int y, int width, int height)
        {
            _xPosition = x;
            _yPosition = y;
            _width = width;
            _height = height;
        }
        public override void DrawScreen()
        {
            DrawBox(_xPosition, _yPosition, _width, _height);
        }

        private void DrawUserInfo(string userName, int score)
        {
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            ClearUserInfo();
            Console.SetCursorPosition(OFFSET_LEFT, _yPosition + 1);
            Console.Write(userName);
            Console.SetCursorPosition(OFFSET_LEFT, _yPosition + 2);
            Console.Write("Score: " + score);
            Console.SetCursorPosition(originalCursorX, originalCursorY);
        }
        private void DrawUserStats(int health, int stamina, int armor, int radiationLevel)
        {
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            ClearUserStats();
            int xpos = (_width / 5) + BORDER_SIZE;
            int ypos = _yPosition + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Health: " + health);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Stamina: " + stamina);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Armor: " + armor);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("RPP: " + radiationLevel);
            Console.SetCursorPosition(originalCursorX, originalCursorY);
        }
        private void DrawUserEquipment(string helm, string body, string weapon)
        {
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            ClearUserEquipment();
            int xpos = (_width / 5) * 2 + BORDER_SIZE;
            int ypos = _yPosition + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Helm: " + helm);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Body: " + body);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Weapon: " + weapon);
            Console.SetCursorPosition(originalCursorX, originalCursorY);
        }
        private void DrawUserInventory(string slotOne, string slotTwo, string slotThree)
        {
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            ClearUserInventory();
            int xpos = (_width / 5) * 3 + BORDER_SIZE;
            int ypos = _yPosition + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 1: " + slotOne);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 2: " + slotTwo);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 3: " + slotThree);
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
        public void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree)
        {
            DrawUserInfo(name, score);
            DrawUserStats(health, stamina, armor, radiation);
            DrawUserEquipment(helm, body, weapon);
            DrawUserInventory(slotOne, slotTwo, slotThree);
        }
        
    }
}
