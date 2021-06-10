using System.Collections.Generic;
using System.Threading;
using ASD_Game.Chat.DTO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASD_Game.UserInterface
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
        private const int INPUT_Y = LOBBY_Y + LOBBY_HEIGHT + BORDER_SIZE;
        private const string INPUT_MESSAGE = "Insert lobby message or command";
        private const int DRAWING_PAUSE = 50;

        private GameChatScreen _gameChatScreen;
        public LobbyScreen()
        {
            _gameChatScreen = new GameChatScreen(CHAT_X, CHAT_Y, CHAT_WIDTH, CHAT_HEIGHT);
        }
        public override void DrawScreen()
        {
            _gameChatScreen.SetScreen(_screenHandler);
            DrawLobbyScreen();
        }

        public void DrawLobbyScreen() 
        {
            DrawHeader(GetHeaderText());
            DrawUserBox();
            _gameChatScreen.DrawScreen();
            DrawLobbyInput(INPUT_MESSAGE);
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
            Thread.Sleep(DRAWING_PAUSE);
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
            _screenHandler.ConsoleHelper.SetCursor(INPUT_X + 4, INPUT_Y + 2);
        }

        public void DrawLobbyInput(string message)
        {
            DrawInputBox(INPUT_X, INPUT_Y, message);
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

        public void ShowMessages(Queue<string> messages)
        {
            _gameChatScreen.ShowMessages(messages);
        }

        public void RedrawInputBox()
        {
            for(int i = INPUT_Y; i < _screenHandler.ConsoleHelper.GetConsoleHeight(); i++)
            {
                _screenHandler.ConsoleHelper.SetCursor(INPUT_X, i);
                _screenHandler.ConsoleHelper.Write(new string(' ', _screenHandler.ConsoleHelper.GetConsoleWidth()));
            }    
            
            DrawLobbyInput(INPUT_MESSAGE);
        }
    }
}