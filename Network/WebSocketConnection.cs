using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Network
{
    public class WebSocketConnector
    {
        private WebSocket _websocket;
        private static readonly string _ip = "localhost";
        private static readonly string _port = "8088";
        private static readonly string _path = "Relay";
        public WebSocketConnector()
        {
            _websocket = new WebSocket($"ws://{_ip}:{_port}/{_path}");
            _websocket.OnMessage += OnMessage;
            _websocket.OnOpen += Websocket_OnOpen;
            _websocket.OnError += Websocket_OnError;
            _websocket.OnClose += Websocket_OnClose;
            _websocket.Connect();
        }

        private void Websocket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine("connection close");
        }

        private void Websocket_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("error event");
        }

        private void Websocket_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("connection open");
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            ObjectPayloadDTO objectPayloadDTO = JsonConvert.DeserializeObject<ObjectPayloadDTO>(e.Data);
            Console.WriteLine("Received: " + objectPayloadDTO.Header.Target);
            Console.WriteLine("Received: " + objectPayloadDTO.Payload);

            if (ObjectPayloadHandler.CheckHeader(objectPayloadDTO.Header))
            {
                ObjectPayloadHandler.CheckActionType(objectPayloadDTO);
            }
        }

        public void Send(string message)
        {
            _websocket.Send(message);
        }
    }
}
