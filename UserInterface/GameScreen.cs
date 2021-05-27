using Session;
using System;

namespace UserInterface
{
    public class GameScreen : Screen
    {
        private const int STAT_X = 0;
        private const int STAT_Y = 0;
        private const int STAT_WIDTH = SCREEN_WIDTH - 2;
        private const int STAT_HEIGHT = 5;

        private const int CHAT_X = 0;
        private const int CHAT_Y = STAT_HEIGHT + 2;
        private const int CHAT_WIDTH = (SCREEN_WIDTH - 2) / 2;
        private const int CHAT_HEIGHT = 10;

        private const int WORLD_X = CHAT_WIDTH + 2;
        private const int WORLD_Y = STAT_HEIGHT + 2;
        private const int WORLD_WITH = (SCREEN_WIDTH - 6) / 2;
        private const int WORLD_HEIGHT = 10;

        private const int INPUT_X = 0;
        private const int INPUT_Y = STAT_HEIGHT + CHAT_HEIGHT + 4;

        private String _userName = "TEMP USERNAME";
        private int _score = 0;
        private int _hp = 100;
        private int _stamina = 100;
        private int _armor = 100;
        private int _radiationProtectionPoints = 100;
        private String _helm = "Bandana";
        private String _body = "Jacket";
        private String _melee = "Knife";
        private String _ranged = "AK-47";
        private String _slotOne = "Bandage";
        private String _slotTwo = "Suspicious white powder";
        private String _slotThree = "Medkit";

        public override void DrawScreen()
        {
            DrawStatBox();
            DrawChatBox();
            DrawWorldBox();
            DrawInputBox(INPUT_X, INPUT_Y, "Insert an option");
        }

        public void DrawStatBox()
        {
            DrawBox(STAT_X, STAT_Y, STAT_WIDTH, STAT_HEIGHT);
            DrawUserInfo();
            DrawUserStats();
            DrawUserEquipment();
            DrawUserInventory();
        }

        public void DrawChatBox()
        {
            DrawBox(CHAT_X, CHAT_Y, CHAT_WIDTH, CHAT_HEIGHT);
        }

        public void DrawWorldBox()
        {
            DrawBox(WORLD_X, WORLD_Y, WORLD_WITH, WORLD_HEIGHT);
        }

        private void DrawUserInfo()
        {
            Console.SetCursorPosition(2, STAT_Y + 1);
            Console.Write(_userName);
            Console.SetCursorPosition(2, STAT_Y + 2);
            Console.Write("Score: " + _score);
        }
        private void DrawUserStats()
        {
            int xpos = (STAT_WIDTH / 5) + 2;
            int ypos = STAT_Y + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("HP: " + _hp);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Stamina: " + _stamina);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Armor: " + _armor);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("RPP: " + _radiationProtectionPoints);
        }
        private void DrawUserEquipment()
        {
            int xpos = (STAT_WIDTH / 5) * 2 + 2;
            int ypos = STAT_Y + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Helm: " + _helm);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Body: " + _body);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Melee: " + _melee);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Ranged: " + _ranged);
        }
        private void DrawUserInventory()
        {
            int xpos = (STAT_WIDTH / 5) * 3 + 2;
            int ypos = STAT_Y + 1;
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 1: " + _slotOne);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 2: " + _slotTwo);
            Console.SetCursorPosition(xpos, ypos++);
            Console.Write("Slot 3: " + _slotThree);

        }
    }
}