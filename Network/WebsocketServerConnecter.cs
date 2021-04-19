using System;
using Newtonsoft.Json;
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
                    Console.WriteLine ("Response from WSserver : " + JsonConvert.DeserializeObject(e.Data));
            
                string json = @"{""method"":""ms.remote.control"",""params"":""{""Cmd"":""Click"",""DataOfCmd"":""KEY_MENU"",""Option"":""false"",""TypeOfRemote"":""SendRemoteKey""}""}";

                string message = JsonConvert.SerializeObject(json);

                ws.Connect();
                ws.Send(message);

                Console.ReadKey();
            }
        }
    }
}