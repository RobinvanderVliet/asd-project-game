using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class GameWorldScreen : Screen
    {
        private int _xPosition;
        private int _yPosition;
        private int _width;
        private int _height;
        private char[,] _map;
        public GameWorldScreen(int x, int y, int width, int height)
        {
            _xPosition = x;
            _yPosition = y;
            _width = width;
            _height = height;
            _map = new char[height, height];
        }

        public override void DrawScreen()
        {
            DrawWorldBox();
            DrawWorld();
        }

        private void DrawWorldBox()
        {
            DrawBox(_xPosition, _yPosition, _width, _height);
        }
        private void DrawWorld()
        {
            int originalCursorX = _screenHandler.ConsoleHelper.GetCursorLeft();
            int originalCursorY = _screenHandler.ConsoleHelper.GetCursorTop();
            ClearWorld();
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                _screenHandler.ConsoleHelper.SetCursor(_xPosition + 1, _yPosition + OFFSET_TOP + i);
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    _screenHandler.ConsoleHelper.Write(_map[i,j].ToString());
                    if(j < _map.GetLength(1) - 1)
                    {
                        _screenHandler.ConsoleHelper.Write("  ");
                    }
                }
            }
            _screenHandler.ConsoleHelper.SetCursor(originalCursorX, originalCursorY);
        }

        private void ClearWorld()
        {
            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                _screenHandler.ConsoleHelper.SetCursor(_xPosition + OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                _screenHandler.ConsoleHelper.Write(new string(' ', _width - 1));
            }
        }
        

        public void UpdateWorld(char[,] newMap)
        {
            _map = newMap;
            DrawWorld();
        }
    }
}
