using System;
using System.Collections.Generic;
using WorldGeneration;

namespace Network.Temporary

// This is a temporary file to use as placeholder since we do not have access to the full session code yet. This code WILL be deleted after the merge with session code, you have been warned.
{
    public class PlaceHolderSessionCode
    {
        
        // The following code should end up in Sessions.
        
        private int _sessionSeed;
        private List<string> joinedClients;
        
        private void GenerateSessionSeed()
        {
            // This function is called when a new session is started to set the seed of that session.
            _sessionSeed = new Random().Next(1, 999999);
            foreach (var client in joinedClients)
            {
                sendPackage(client, _sessionSeed);
            }
        }

        private void sendPackage(string client, int sessionSeed)
        {
            throw new NotImplementedException();
        }

        // The following code should end up at the receiving end of the messages:
        
        private Map gameMap;

        private void SetMapSeed(int seed)
        {
            // Note that this creates the map that will be used by the players in the game and overwrites the old map object. So this call should only be done with the game starts.
            gameMap = MapFactory.GenerateMap(seed: seed);
            // Set the map used by the game as the generated map.
        }
        

    }
}