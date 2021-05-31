using System;

namespace UserInterface
{
    public class GameScreen : Screen
    {
        private GameStatScreen _gameStatScreen;
        private GameChatScreen _gameChatScreen;
        private GameWorldScreen _gameWorldScreen;

        private const int STAT_X = HEADER_X;
        private const int STAT_Y = HEADER_Y;
        private const int STAT_WIDTH = HEADER_WIDTH;
        private const int STAT_HEIGHT = 5;

        private const int CHAT_X = HEADER_X;
        private const int CHAT_Y = STAT_HEIGHT + BORDER_SIZE;
        private const int CHAT_WIDTH = (SCREEN_WIDTH - BORDER_SIZE) - (WORLD_WITH + BORDER_SIZE);
        private const int CHAT_HEIGHT = 13;

        private const int WORLD_X = CHAT_WIDTH + BORDER_SIZE;
        private const int WORLD_Y = STAT_HEIGHT + BORDER_SIZE;
        
        private const int WORLD_HEIGHT = 13;
        private const int WORLD_WITH = 25;
        private const int INPUT_X = HEADER_X;
        private const int INPUT_Y = STAT_HEIGHT + CHAT_HEIGHT + (BORDER_SIZE * 2);

        public GameScreen()
        {
            _gameStatScreen = new GameStatScreen(STAT_X, STAT_Y, STAT_WIDTH, STAT_HEIGHT);
            _gameChatScreen = new GameChatScreen(CHAT_X, CHAT_Y, CHAT_WIDTH, CHAT_HEIGHT);
            _gameWorldScreen = new GameWorldScreen(WORLD_X, WORLD_Y, WORLD_WITH, WORLD_HEIGHT);
        }

        public override void DrawScreen()
        {
            _gameStatScreen.DrawScreen();
            _gameChatScreen.DrawScreen();
            _gameWorldScreen.DrawScreen();
            DrawInputBox(INPUT_X, INPUT_Y, "Insert an option");
            test();
        } 

        public void AddMessage(string message)
        {
            if (_screenHandler.Screen is GameScreen)
            {
                _gameChatScreen.AddMessage(message);
                DrawInputBox(INPUT_X, INPUT_Y, "Insert an option");
            }
        }

        public void SetStartValues(string name, string score, string health, string stamina, string armor, string radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree)
        {
            _gameStatScreen.SetStartValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree);
        }

        public void UpdateStat(string UpdatedStat, string newValue)
        {
            _gameStatScreen.UpdateStat(UpdatedStat, newValue);
        }

        public void test()
        {
         String _userName = "TEMP USERNAME";
         String _score = "0";
         String _health = "100";
         String _stamina = "100";
         String _armor = "100";
         String _radiationProtectionPoints = "100";
         String _helm = "Bandana";
         String _body = "Jacket";
         String _weapon = "Knife";
         String _slotOne = "Bandage";
         String _slotTwo = "Suspicious white powder";
         String _slotThree = "Medkit";
         SetStartValues(_userName,_score, _health, _stamina, _armor, _radiationProtectionPoints, _helm, _body, _weapon, _slotOne, _slotTwo, _slotThree);
            System.Threading.Thread.Sleep(1000);
            UpdateStat("Name", "TestUser");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("Score", "420");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("Health", "90");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("Stamina", "80");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("Armor", "70");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("Radiation", "60");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("Helm", "Military helmet");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("Body", "Tactical vest");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("Weapon", "Katana");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("SlotOne", "Morphine");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("SlotTwo", "Monster energy");
            System.Threading.Thread.Sleep(1000);
            UpdateStat("SlotThree", "Suspicious white powder");
        }
    }
}