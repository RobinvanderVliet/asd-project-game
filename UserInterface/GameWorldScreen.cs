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

        public GameWorldScreen(int x, int y, int width, int height)
        {
            _xPosition = x;
            _yPosition = y;
            _width = width;
            _height = height;
        }

        public override void DrawScreen()
        {
            DrawWorldBox();
        }

        public void DrawWorldBox()
        {
            DrawBox(_xPosition, _yPosition, _width, _height);
        }
    }
}
