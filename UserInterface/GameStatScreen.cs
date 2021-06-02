using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{

    public class GameStatScreen : Screen
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

        public GameStatScreen()
        {

        }

        public override void DrawScreen()
        {
            DrawBox(_xPosition, _yPosition, _width, _height);
        }

        private void DrawUserInfo(string userName, int score)
        {
            _screenHandler.ConsoleHelper.SetCursor(OFFSET_LEFT, _yPosition + 1);
            _screenHandler.ConsoleHelper.Write(userName);
            _screenHandler.ConsoleHelper.SetCursor(OFFSET_LEFT, _yPosition + 2);
            _screenHandler.ConsoleHelper.Write("Score: " + score);
        }
        private void DrawUserStats(int health, int stamina, int armor, int radiationLevel)
        {
            int xpos = (_width / 5) + BORDER_SIZE;
            int ypos = _yPosition + 1;
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("Health: " + health);
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("Stamina: " + stamina);
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("Armor: " + armor);
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("RPP: " + radiationLevel);
        }
        private void DrawUserEquipment(string helm, string body, string weapon)
        {
            int xpos = (_width / 5) * 2 + BORDER_SIZE;
            int ypos = _yPosition + 1;
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("Helm: " + helm);
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("Body: " + body);
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("Weapon: " + weapon);
        }
        private void DrawUserInventory(string slotOne, string slotTwo, string slotThree)
        {
            int xpos = (_width / 5) * 3 + BORDER_SIZE;
            int ypos = _yPosition + 1;
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("Slot 1: " + slotOne);
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("Slot 2: " + slotTwo);
            _screenHandler.ConsoleHelper.SetCursor(xpos, ypos++);
            _screenHandler.ConsoleHelper.Write("Slot 3: " + slotThree);
        }

        private void ClearAllStats()
        {
            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                _screenHandler.ConsoleHelper.SetCursor(OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                _screenHandler.ConsoleHelper.Write(new string(' ', _width - BORDER_SIZE));
            }
        }
        
        public virtual void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree)
        {
            int originalCursorX = _screenHandler.ConsoleHelper.GetCursorLeft();
            int originalCursorY = _screenHandler.ConsoleHelper.GetCursorTop();
            ClearAllStats();
            DrawUserInfo(name, score);
            DrawUserStats(health, stamina, armor, radiation);
            DrawUserEquipment(helm, body, weapon);
            DrawUserInventory(slotOne, slotTwo, slotThree);
            _screenHandler.ConsoleHelper.SetCursor(originalCursorX, originalCursorY);
        }
        
    }
}
