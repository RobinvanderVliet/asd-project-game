using System;
using Chat;
using Player.Model;
using Session;

namespace Player.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerModel _playerModel;
        private readonly IChatHandler _chatHandler;
        private readonly ISessionHandler _sessionHandler;
        private const int DEFAULT_STEPS = 0;

        public PlayerService(IPlayerModel playerModel, IChatHandler chatHandler, ISessionHandler sessionHandler)
        {
            _playerModel = playerModel;
            _chatHandler = chatHandler;
            _sessionHandler = sessionHandler;
        }

        public void Attack(string direction)
        {
            //player1.getTile();
            //check with the gameboard whether or not there's a player in the given direction from this tile
            //player2 = tile(x,y).getPlayer
            //if yes {
            //int dmg = player1.GetAttackDamage();
            //player1.RemoveStamina(1);
            // player2.RemoveHealth(dmg);
            //} else {  
            //  Console.WriteLine("You swung at nothing!");
            // player1.RemoveStamina(1);
            //}
            Console.WriteLine("Attacked in " + direction + " direction.");
        }

        public void ExitCurrentGame()
        {
            //code for removing player from lobby
            Console.WriteLine("Game left.");
        }

        public void Pause()
        {
            //code for pause game (only for host)
            Console.WriteLine("Game paused.");
        }

        public void Resume()
        {
            //code for resume game (only for host)
            Console.WriteLine("Game resumed.");
        }

        public void ReplaceByAgent()
        {
            //code for replacing player by agent
            Console.WriteLine("Replaced by agent");
        }

        public void Say(string messageValue)
        {
            _chatHandler.SendSay(messageValue);
            //code for chat with other players in team chat
            Console.WriteLine(_playerModel.Name + " sent message: " + messageValue);
        }

        public void Shout(string messageValue)
        {
            //code for chat with other players in general chat
            Console.WriteLine(_playerModel.Name + " sent message: " + messageValue);
        }
        
        public void AddHealth(int amount)
        {
            _playerModel.AddHealth(amount);
        }

        public void RemoveHealth(int amount)
        {
            _playerModel.RemoveHealth(amount);
        }

        public void AddStamina(int amount)
        {
            _playerModel.AddStamina(amount);
        }

        public void RemoveStamina(int amount)
        {
            _playerModel.RemoveStamina(amount);
        }

        public IItem GetItem(string itemName)
        {
            return _playerModel.GetItem(itemName);
        }

        public void AddInventoryItem(IItem item)
        {
            _playerModel.AddInventoryItem(item);
        }

        public void RemoveInventoryItem(IItem item)
        {
            _playerModel.RemoveInventoryItem(item);
        }

        public void EmptyInventory()
        {
            _playerModel.EmptyInventory();
        }

        public void AddBitcoins(int amount)
        {
            _playerModel.AddBitcoins(amount);
        }

        public void RemoveBitcoins(int amount)
        {
            _playerModel.RemoveBitcoins(amount);
        }

        public int GetAttackDamage()
        {
            return _playerModel.GetAttackDamage();
        }
        
        public void PickupItem()
        {
            _playerModel.PickupItem();
        }

        public void DropItem(string itemNameValue)
        {
            _playerModel.DropItem(itemNameValue);
        }
        
        public void HandleDirection(string directionValue, int stepsValue)
        {
            var newMovement = new int[2];
            switch (directionValue)
            {
                case "right":
                case "east":
                    newMovement[0] = stepsValue;
                    newMovement[1] = DEFAULT_STEPS;
                    break;
                case "left":
                case "west":
                    newMovement[0] = -stepsValue;
                    newMovement[1] = DEFAULT_STEPS;
                    break;
                case "forward":
                case "up":
                case "north":
                    newMovement[0] = DEFAULT_STEPS;
                    newMovement[1] = -stepsValue;
                    break;
                case "backward":
                case "down":
                case "south":
                    newMovement[0] = DEFAULT_STEPS;
                    newMovement[1] = stepsValue;
                    break;
            }

            _playerModel.SetNewPlayerPosition(newMovement);

            // the next line of code should be changed by sending newPosition to a relevant method
            WriteCommand(_playerModel.CurrentPosition);
        }

        public void CreateSession(string messageValue)
        {
            _sessionHandler.CreateSession(messageValue);
        }

        public void JoinSession(string messageValue)
        {
            _sessionHandler.JoinSession(messageValue);
        }

        public void RequestSessions()
        {
            _sessionHandler.RequestSessions();
        }

        // !!! METHODS BELOW ARE TEMPORARY, PROTOTYPE ONLY !!!
        private void WriteCommand(int[] newPosition)
        {
            // returns the new position
            _playerModel.CurrentPosition = newPosition;
            Console.WriteLine("X: " + newPosition[0] + ". Y: " + newPosition[1]);
        }
    }
}