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
            ClientComponent clientComponent = new ClientComponent(networkComponent);
            networkComponent.Client = clientComponent;
            
            if (roleResult != null && roleResult.ToLower() == "host")
            {
                Console.WriteLine("Name of your session?");
                clientComponent.CreateGame(Console.ReadLine());
                
                Console.Read();
            }
            else if (roleResult != null && roleResult.ToLower() == "client")
            {
                clientComponent.FindGames();
                Console.WriteLine("Type the sessionID of the game you would like to join.");
                clientComponent.JoinGame(Console.ReadLine());
                
                Console.Read();
            }
        }
    }
}
