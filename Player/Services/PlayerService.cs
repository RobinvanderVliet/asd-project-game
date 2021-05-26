using System;
using System.Collections.Generic;
using Player.ActionHandlers;
using Chat;
using DataTransfer.DTO.Character;
using Network;
using Player.Model;
using WorldGeneration;

namespace Player.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerModel _currentPlayer;
        private List<MapCharacterDTO> _playerPositions;
        private readonly IMoveHandler _moveHandler;
        private readonly IChatHandler _chatHandler;
        private readonly IWorldService _worldService;
        private readonly IClientController _clientController;

        public PlayerService(IPlayerModel currentPlayer
            , IChatHandler chatHandler
            , IMoveHandler moveHandler
            , IClientController clientController
            , IWorldService worldService)
        {
            _chatHandler = chatHandler;
            _currentPlayer = currentPlayer;
            _moveHandler = moveHandler;
            _clientController = clientController;
            currentPlayer.PlayerGuid = _clientController.GetOriginId();
            _worldService = worldService;
        }

        public void Slash(string direction)
        {
          Console.WriteLine("Slashed in " + direction + " direction.");
        }

        public void Shoot(string direction)
        {
            Console.WriteLine("Shot in " + direction + " direction.");
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
        }

        public void Shout(string messageValue)
        {
            _chatHandler.SendShout(messageValue);
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

            
            var mapCharacterDTO = new MapCharacterDTO((_worldService.getCurrentCharacterPositions().XPosition) + x, 
                (_worldService.getCurrentCharacterPositions().YPosition) + y, 
                _currentPlayer.PlayerGuid, 
                _worldService.getCurrentCharacterPositions().GameGuid, 
                _currentPlayer.Symbol);
            
            _moveHandler.SendMove(mapCharacterDTO);
            
            MapCharacterDTO currentCharacter =  _worldService.getCurrentCharacterPositions();
           _currentPlayer.XPosition = currentCharacter.XPosition;
           _currentPlayer.YPosition = currentCharacter.YPosition;
        }
    }
}