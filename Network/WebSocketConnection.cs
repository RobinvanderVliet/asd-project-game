using Newtonsoft.Json;
using System;
using WebSocketSharp;

namespace Network
{
    public class WebSocketConnection
    {
        private WebSocket _websocket;
        private readonly string _ip = "localhost";
        private readonly string _port = "8088";
        private readonly string _path = "Relay";
        public WebSocketConnection()
        {
            _websocket = new WebSocket($"ws://{_ip}:{_port}/{_path}");
            AddBehaviorToWebsocket();
            try
            {
                _websocket.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public WebSocketConnection(WebSocket webSocket)
        {
            this._websocket = webSocket;
            AddBehaviorToWebsocket();
            _websocket.Connect();
        }

        private void AddBehaviorToWebsocket()
        {
            _websocket.OnMessage += OnMessage;
            _websocket.OnOpen += Websocket_OnOpen;
            _websocket.OnError += Websocket_OnError;
            _websocket.OnClose += Websocket_OnClose;
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
            try
            {
                _websocket.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
