using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    class GameWorldScreen : Screen
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
            _map = new char[13, 13] {
            {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','☻','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
             {'.','.','~','~','~','~','~','~','~','~','~','~','~'},
            };
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
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            ClearWorld();
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                Console.SetCursorPosition(_xPosition + 1, _yPosition + OFFSET_TOP + i);
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    Console.Write(_map[i,j]);
                    if(j < _map.GetLength(1) - 1)
                    {
                        Console.Write("  ");
                    }
                }
            }
            Console.SetCursorPosition(originalCursorX, originalCursorY);
        }

        private void ClearWorld()
        {
            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                Console.SetCursorPosition(_xPosition + OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                Console.Write(new string(' ', _width - 1));
            }
        }
        

        public void UpdateWorld(char[,] newMap)
        {
            _map = newMap;
            DrawWorld();
        }
    }
}
