using System;
using DataTransfer.DTO.Character;

namespace DataTransfer.DTO.Player
{
    public class PlayerDTO : CharacterDTO
    {
        public PlayerDTO(
            int xPosition
            , int yPosition
            , string playerGuid
            , string gameGuid
            , string symbol
            ,  ConsoleColor color = ConsoleColor.White
            , ConsoleColor backgroundColor = ConsoleColor.Black
            , int team = 0
            , int health = default
            , int stamina = default
            , int radiationLevel = default
            , InventoryDTO inventory = null
            , BitcoinDTO bitcoins = null) 
            : base(xPosition
                , yPosition
                , playerGuid
                , gameGuid
                , symbol,  color
                , backgroundColor
                , team
                , health
                , stamina
                , radiationLevel
                , inventory
                , bitcoins)
        {
        }
    }
}