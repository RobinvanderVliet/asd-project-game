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

        public GameChatScreen(int x, int y, int width, int height)
        {
            _xPosition = x;
            _yPosition = y;
            _width = width;
            _height = height;
        }

        public GameChatScreen()
        {

        }

        public override void DrawScreen()
        {
            DrawChatBox();          
        }

        private void DrawChatBox()
        {
            DrawBox(_xPosition, _yPosition, _width, _height);
        }

        private void DrawMessages(Queue<string> messageQueue)
        {
            int originalCursorX = _screenHandler.ConsoleHelper.GetCursorLeft();
            int originalCursorY = _screenHandler.ConsoleHelper.GetCursorTop();
            ClearMessages();
            int messageCount = messageQueue.Count;
            for (int i = 0; i < messageCount; i++)
            {
                string message = messageQueue.Peek();
                _screenHandler.ConsoleHelper.SetCursor(_xPosition + OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                _screenHandler.ConsoleHelper.Write(message);
                messageQueue.Dequeue();
            }
            _screenHandler.ConsoleHelper.SetCursor(originalCursorX, originalCursorY);
        }

        private void ClearMessages()
        {
            for (int i = 0; i <= _height - OFFSET_TOP; i++)
            {
                _screenHandler.ConsoleHelper.SetCursor(_xPosition + OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                _screenHandler.ConsoleHelper.Write(new string(' ', _width - BORDER_SIZE));
            }
        }

        public void ShowMessages(Queue<string> messages)
        {
            Queue<string> messageQueue = new Queue<string>();
            int messageCount = messages.Count;
            for (int i = 0; i < messageCount; i++)
            {
                string message = messages.Dequeue();
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

                    for (int j = 0; j < stringLength; j += chunkSize)
                    {
                        if (j + chunkSize > stringLength)
                        {
                            chunkSize = stringLength - j;
                        }
                        messageQueue.Enqueue(message.Substring(j, chunkSize));

                        if (messageQueue.Count > _height)
                        {
                            messageQueue.Dequeue();
                        }
                    }
                }
                else
                {
                    messageQueue.Enqueue(message);
                }
                if (messageQueue.Count > _height)
                {
                    messageQueue.Dequeue();
                }
            }
            DrawMessages(messageQueue);
        }
    }
}
