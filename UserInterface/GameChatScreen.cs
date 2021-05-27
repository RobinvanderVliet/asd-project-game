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
        private Queue<string> messages = new Queue<string>();

        public GameChatScreen(int x, int y, int width, int height)
        {
            _xPosition = x;
            _yPosition = y;
            _width = width;
            _height = height;

            //temp test
            AddMessage("Test 1");
            AddMessage("Test 2");
            AddMessage("Test 3");
            AddMessage("Test 4");
            AddMessage("Test 5");
            AddMessage("Test 6");
            AddMessage("Test 7");
            AddMessage("Test 8");
            AddMessage("Test 9");
            AddMessage("Test 10");
        }

        public override void DrawScreen()
        {
            DrawChatBox();
            DrawMessages();
        }

        private void DrawChatBox()
        {
            DrawBox(_xPosition, _yPosition, _width, _height);
        }

        private void DrawMessages()
        {
            Queue<string> tempMessages = new Queue<string>();
            ClearMessages();
            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                string message = messages.Dequeue();
                Console.SetCursorPosition(_xPosition + OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                Console.Write(message);
                tempMessages.Enqueue(message);
            }
            messages = tempMessages;
        }

        private void ClearMessages()
        {
            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                Console.SetCursorPosition(_xPosition + OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                Console.Write(new string(' ', _width - BORDER_SIZE));
            }
        }

        public void AddMessage(string message)
        {
            if(messages.Count == _height)
            {
                messages.Dequeue();
            }
            messages.Enqueue(message);
        }
    }
}
