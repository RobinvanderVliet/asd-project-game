using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    class GameChatScreen : Screen
    {
        private int _xPosition;
        private int _yPosition;
        private int _width;
        private int _height;

        public GameChatScreen(int x, int y, int width, int height)
        {
            _xPosition = x;
            _yPosition = y;
            _width = width;
            _height = height;
        }

        public override void DrawScreen()
        {
            DrawChatBox();
        }

        private void DrawChatBox()
        {
            DrawBox(_xPosition, _yPosition, _width, _height);
        }

    }
}
