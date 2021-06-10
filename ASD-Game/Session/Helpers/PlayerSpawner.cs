using System.Collections.Generic;
using ASD_Game.Network;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Services;

namespace ASD_Game.Session.Helpers
{
    public static class PlayerSpawner
    {
        public static Player SpawnPlayers(IEnumerable<string[]> allClients, int spawnSeed, IWorldService worldService, IClientController clientController)
        {
            // spawn position first person.
            var playerX = spawnSeed % 50; 
            var playerY = spawnSeed % 50; 
            Player currentPlayer = null;
            foreach (var client in allClients)
            {
                var canSpawnOnTile = false;
                while (!canSpawnOnTile)
                {
                    // changes the X and y coordinates based on the seed.
                    playerX = ChangePlayerX(spawnSeed, playerX);
                    playerY = ChangePlayerY(spawnSeed, playerY);

                    canSpawnOnTile = CheckIfPlayerCanSpawnOnTile(worldService, playerX, playerY);
                }
            
                if (clientController.GetOriginId() == client[0])
                {
                    currentPlayer = new Player(client[1], playerX, playerY, CharacterSymbol.CURRENT_PLAYER, client[0]);
                    worldService.AddPlayerToWorld(currentPlayer, true);
                }
                else
                {
                    var playerObject = new Player(client[1], playerX, playerY, CharacterSymbol.ENEMY_PLAYER, client[0]);
                    worldService.AddPlayerToWorld(playerObject, false);
                }
            }
            return currentPlayer;
        }

        private static bool CheckIfPlayerCanSpawnOnTile(IWorldService worldService, int playerX, int playerY)
        {
            worldService.LoadArea(playerX, playerY, 0);
            var tile = worldService.GetTile(playerX, playerY);
            return tile is ITerrainTile && tile.IsAccessible && !worldService.CheckIfCharacterOnTile(tile);
        }

        private static int ChangePlayerX(int spawnSeed, int playerX)
        {
            if (playerX % 2 == 0)
            {
                playerX += spawnSeed % 3;
            }
            else
            {
                playerX -= spawnSeed % 3;
            }
            return playerX +1;
        }
        
        private static int ChangePlayerY(int spawnSeed, int playerY)
        {
            if (playerY % 2 == 0)
            {
                playerY += spawnSeed % 5;
            }
            else
            {
                playerY -= spawnSeed % 5;
            }
            return playerY +1;
        }
        
        
        

        
    }
}