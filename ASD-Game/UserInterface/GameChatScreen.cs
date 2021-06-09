using System;
using System.Collections.Generic;

namespace ASD_Game.UserInterface
{
    public class GameChatScreen : Screen, IGameChatScreen
    {
        private readonly int _xPosition;
        private readonly int _yPosition;
        private readonly int _width;
        private readonly int _height;      

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

        private void DrawMessages(Stack<string> messageQueue)
        {
            int originalCursorX = _screenHandler.ConsoleHelper.GetCursorLeft();
            int originalCursorY = _screenHandler.ConsoleHelper.GetCursorTop();
            ClearMessages();
            int messageCount = messageQueue.Count;
            for (int i = 0; i < messageCount; i++)
            {
                string message = messageQueue.Pop();
                _screenHandler.ConsoleHelper.SetCursor(_xPosition + OFFSET_LEFT, _yPosition + OFFSET_TOP + i);
                _screenHandler.ConsoleHelper.Write(message);
            }
            _screenHandler.ConsoleHelper.SetCursor(originalCursorX, originalCursorY);
        }

        private void ClearMessages()
        {
            for (int i = 0; i <= _height + 1; i++)
            {
                _screenHandler.ConsoleHelper.SetCursor(_xPosition, _yPosition + i);
                _screenHandler.ConsoleHelper.Write(new string(' ', _width + BORDER_SIZE));
            }
            DrawScreen();
        }

        public void ShowMessages(Queue<string> messages)
        {
            Stack<string> messageStack = new();
            int totalMessage = messages.Count;
            for (int i = 0; i < totalMessage; i++)
            {
                string message = messages.Dequeue();
                var messageSplitted = message.Split(Environment.NewLine);

                for (int k = messageSplitted.Length - 1; k >= 0; k--)
                {
                    message = messageSplitted[k];
                    if (message.Length >= _width - BORDER_SIZE)
                    {
                        int stringLength = message.Length;
                        int chunkSize = _width - BORDER_SIZE;
                        int maxSize = chunkSize * _height;

                        if (stringLength > maxSize)
                        {
                            message = message.Substring(0, maxSize - 3) + "...";
                            stringLength = maxSize;
                        }

                        Stack<string> tempStack = new();
                        for (int j = 0; j < stringLength; j += chunkSize)
                        {
                            if (j + chunkSize > stringLength)
                            {
                                chunkSize = stringLength - j;
                            }
                            tempStack.Push(message.Substring(j, chunkSize));

                            if (messageStack.Count + tempStack.Count > _height)
                            {
                                tempStack.Pop();
                            }
                        }
                        foreach(string line in tempStack)
                        {
                            messageStack.Push(line);
                        }
                    }
                    else
                    {
                        messageStack.Push(message);
                    }

                    if (messageStack.Count > _height)
                    {
                        messageStack.Pop();
                    }

                }
            }
            DrawMessages(messageStack);
        }
    }
}