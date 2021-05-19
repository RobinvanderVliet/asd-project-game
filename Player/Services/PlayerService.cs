using System;
using System.Collections.Generic;
using Player.ActionHandlers;
using Player.DTO;
using Chat;
using DataTransfer.DTO.Character;
using DataTransfer.DTO.Player;
using Player.Model;
using Session;
using WorldGeneration;
using WorldGeneration.Models;

namespace Player.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerModel _currentPlayer;
        private List<MapCharacterDTO> _playerPositions;
        private readonly IMoveHandler _moveHandler;
        private readonly IChatHandler _chatHandler;
        //session handler in aparte classe gebruiken, kan maybe blijven staan? Weet niet of die nog gebrukt gaat worden. :(
        private readonly ISessionHandler _sessionHandler;
        private readonly WorldService _worldService;

        public PlayerService(IPlayerModel currentPlayer
            , IChatHandler chatHandler
            , ISessionHandler sessionHandler
            , List<MapCharacterDTO> playerPositions
            , IMoveHandler moveHandler
            , WorldService worldService)
        {
            _chatHandler = chatHandler;
            _sessionHandler = sessionHandler;
            _currentPlayer = currentPlayer;
            _playerPositions = playerPositions;
            _moveHandler = moveHandler;
            _worldService = worldService;
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
            //Console.WriteLine(_currentPlayer.Name + " sent message: " + messageValue);
        }

        public void Shout(string messageValue)
        {
            //code for chat with other players in general chat
            Console.WriteLine(_currentPlayer.Name + " sent message: " + messageValue);
        }

        public void AddHealth(int amount)
        {
            _currentPlayer.AddHealth(amount);
        }

        public void RemoveHealth(int amount)
        {
            _currentPlayer.RemoveHealth(amount);
        }

        public void AddStamina(int amount)
        {
            _currentPlayer.AddStamina(amount);
        }

        public void RemoveStamina(int amount)
        {
            _currentPlayer.RemoveStamina(amount);
        }

        public IItem GetItem(string itemName)
        {
            return _currentPlayer.GetItem(itemName);
        }

        public void AddInventoryItem(IItem item)
        {
            _currentPlayer.AddInventoryItem(item);
        }

        public void RemoveInventoryItem(IItem item)
        {
            _currentPlayer.RemoveInventoryItem(item);
        }

        public void EmptyInventory()
        {
            _currentPlayer.EmptyInventory();
        }

        public void AddBitcoins(int amount)
        {
            _currentPlayer.AddBitcoins(amount);
        }

        public void RemoveBitcoins(int amount)
        {
            _currentPlayer.RemoveBitcoins(amount);
        }

        public int GetAttackDamage()
        {
            return _currentPlayer.GetAttackDamage();
        }

        public void PickupItem()
        {
            _currentPlayer.PickupItem();
        }

        public void DropItem(string itemNameValue)
        {
            _currentPlayer.DropItem(itemNameValue);
        }

        public void HandleDirection(string directionValue, int stepsValue)
        {
            int x = 0;
            int y = 0;
            switch (directionValue)
            {
                case "right":
                case "east":
                    x = stepsValue;
                    break;
                case "left":
                case "west":
                    x = -stepsValue;
                    break;
                case "forward":
                case "up":
                case "north":
                    y = +stepsValue;
                    break;
                case "backward":
                case "down":
                case "south":
                    y = -stepsValue;
                    break;
            }

            
            _currentPlayer.SetNewPlayerPosition(x, y);
            //MapCharacterDTO dto = new CharacterDTO(0, 0, _currentPlayer.);
            // foreach (var player in _playerPositions)
            // {
            //     if (player.Name.Equals(_currentPlayer.Name))
            //     {
            //         dto = player;
            //     }
            // }
            var dto = new MapCharacterDTO(_currentPlayer.XPosition, _currentPlayer.YPosition, _currentPlayer.Name, _currentPlayer.Symbol);

            _moveHandler.SendMove(dto, _worldService);
            

            // the next line of code should be changed by sending newPosition to a relevant method
            Console.WriteLine("X: " + _currentPlayer.XPosition + ". Y: " + _currentPlayer.YPosition);
        }
        
        public void ChangePositionOfAPlayer(PlayerPositionDTO playerPosition)
        {
            foreach (var player in _playerPositions)
            {
                if (player.Name == playerPosition.PlayerName)
                {
                    playerPosition.X = player.XPosition;
                    playerPosition.Y = player.YPosition;
                    
                }
            }
        }
        
     
    }
}