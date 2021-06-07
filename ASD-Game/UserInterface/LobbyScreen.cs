using Chat.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class LobbyScreen : Screen
    {
        private const int LOBBY_X = SCREEN_WIDTH / 2;
        private const int LOBBY_Y = HEADER_HEIGHT + BORDER_SIZE;
        private const int LOBBY_WIDTH = SCREEN_WIDTH / 2 - BORDER_SIZE;
        private const int LOBBY_HEIGHT = 8;

        private const int CHAT_X = 0;
        private const int CHAT_Y = HEADER_HEIGHT + BORDER_SIZE;
        private const int CHAT_WIDTH = LOBBY_WIDTH;
        private const int CHAT_HEIGHT = 8;

        private const int INPUT_X = 0;
        private const int INPUT_Y = LOBBY_Y + BORDER_SIZE;
        private const string INPUT_MESSAGE = "Insert lobby message or command";
        public override void DrawScreen()
        {
            DrawLobbyScreen();
        }

        public void DrawLobbyScreen()
        {
            DrawHeader(GetHeaderText());
            DrawUserBox();
            DrawChatBox();
            UpdateChat(UpdateMessages());
            DrawLobbyInput(INPUT_MESSAGE);
        }

        private void DrawChatBox()
        {
            DrawBox(CHAT_X, CHAT_Y, CHAT_WIDTH, CHAT_HEIGHT);
        }

        private void DrawUserBox()
        {
            DrawBox(LOBBY_X, LOBBY_Y, LOBBY_WIDTH, LOBBY_HEIGHT);
        }

        private string GetHeaderText()
        {
            return "Welcome to the lobby, people in the lobby:";
        }

        public virtual void UpdateLobbyScreen(List<string[]> clients)
        {
            foreach (string[] client in clients)
            {
                int position = clients.IndexOf(client);
                _screenHandler.ConsoleHelper.SetCursor(LOBBY_X + 1, LOBBY_Y + position);
                _screenHandler.ConsoleHelper.SetCursor(LOBBY_X + BORDER_SIZE / 2, LOBBY_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write(" ");
                _screenHandler.ConsoleHelper.SetCursor(LOBBY_X + OFFSET_LEFT, LOBBY_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write(position + 1 + ". " + client[1]);
            }

            ResetCursor();
        }

        public void ResetCursor()
        {
            _screenHandler.ConsoleHelper.SetCursor(INPUT_X + 4, INPUT_Y + LOBBY_HEIGHT + 2);
        }

        public void DrawLobbyInput(string message)
        {
            DrawInputBox(INPUT_X, INPUT_Y + LOBBY_HEIGHT, message);
        }

        public void UpdateChat(List<ChatMessageDTO> messages)
        {
            foreach (ChatMessageDTO message in messages)
            {
                int position = messages.IndexOf(message);
                _screenHandler.ConsoleHelper.SetCursor(CHAT_X + 1, CHAT_Y + position);
                _screenHandler.ConsoleHelper.SetCursor(CHAT_X + BORDER_SIZE / 2, CHAT_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write(" ");
                _screenHandler.ConsoleHelper.SetCursor(CHAT_X + OFFSET_LEFT, CHAT_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write(message.UserName + " : " + message.Message);
            }
        }

        //REMOVE THIS FUNCTION WHEN CHAT HAS BEEN FULLY IMPLEMENTED
        public List<ChatMessageDTO> UpdateMessages()
        {
            List<ChatMessageDTO> list = new();
            list.Add(new ChatMessageDTO("swankie", "this is the first message"));
            list.Add(new ChatMessageDTO("jeroen", "this is the second message"));
            return list;
        }
    }
}
