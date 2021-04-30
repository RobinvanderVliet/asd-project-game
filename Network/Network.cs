using System;

namespace Network
{
    class Network
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Are you a Client or Host?");

            String roleResult = Console.ReadLine();

            NetworkComponent networkComponent = new NetworkComponent();
            ClientController clientController = new ClientController(networkComponent);
            networkComponent.Client = clientController;
            
            if (roleResult != null && roleResult.ToLower() == "host")
            {
                Console.WriteLine("Name of your session?");
                clientController.CreateGame(Console.ReadLine());
                
                Console.Read();
            }
            else if (roleResult != null && roleResult.ToLower() == "client")
            {
                clientController.FindGames();
                Console.WriteLine("Type the sessionID of the game you would like to join.");
                clientController.JoinGame(Console.ReadLine());
                
                Console.Read();
            }
        }
    }
}
