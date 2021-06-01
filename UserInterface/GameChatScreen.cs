using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class GameChatScreen : Screen
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
            int originalCursorX = Console.CursorLeft;
            int originalCursorY = Console.CursorTop;
            Queue<string> tempMessages = new Queue<string>();
            ClearMessages();
            int messageCount = messages.Count;
            for (int i = 0; i < messageCount; i++)
            {
                string message = messages.Dequeue();
                Console.SetCursorPosition(_xPosition + OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                Console.Write(message);
                tempMessages.Enqueue(message);
            }
            messages = tempMessages;
            Console.SetCursorPosition(originalCursorX, originalCursorY);
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
            if (message.Length >= _width - BORDER_SIZE)
            {
                int chunkSize = _width - BORDER_SIZE;
                int stringLength = message.Length;
                int maxSize = chunkSize * _height;
                if (stringLength > maxSize)
                {
                    message = message.Substring(0, maxSize - 3) + "...";
                    stringLength = maxSize;
                }

                for (int i = 0; i < stringLength; i += chunkSize)
                {
                    if (i + chunkSize > stringLength)
                    {
                        chunkSize = stringLength - i;
                    }
                    messages.Enqueue(message.Substring(i, chunkSize));

                    if (messages.Count > _height)
                    {
                        messages.Dequeue();
                    }
                }
            }
            else
            {
                messages.Enqueue(message);
            }
            if (messages.Count > _height)
            {
                messages.Dequeue();
            }
            DrawMessages();
        }
    }
}
