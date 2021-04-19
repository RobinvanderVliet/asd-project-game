using System;
using WebSocketSharp;

namespace Network
{
    public class WebsocketServerConnecter
    {
        static void Main(string[] args)
        {
            //Creating of a disposable WsClient instance
            using (WebSocket ws = new WebSocket("ws://localhost:8088/Echo"))
            {
                ws.OnMessage += (sender, e) =>
                    Console.WriteLine ("Response from WSserver : " + e.Data);
            
                ws.Connect();
                ws.Send("hello from client");

                Console.ReadKey();
            }
        }
    }
}