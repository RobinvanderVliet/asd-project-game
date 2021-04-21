using System;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Network
{
    class WebsocketServerConnector
    {
        static void Main(string[] args)
        {
            //Creating of a disposable WsClient instance
            using (WebSocket ws = new WebSocket("ws://localhost:8088/Echo"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    ObjectPayloadDTO objectPayloadDTO2 = JsonConvert.DeserializeObject<ObjectPayloadDTO>(e.Data);
                    Console.WriteLine("Received: " + objectPayloadDTO2.Header.Target);
                    Console.WriteLine("Received: " + objectPayloadDTO2.Payload);

                    ObjectPayloadHandler objectPayloadHandler = new ObjectPayloadHandler();

                    if(objectPayloadHandler.checkHeader(objectPayloadDTO2.Header))
                    {
                        objectPayloadHandler.checkActionType(objectPayloadDTO2);
                    }
                };

                PayloadHeaderDTO payloadHeaderDTO = new PayloadHeaderDTO();
                payloadHeaderDTO.Target = "host";
                payloadHeaderDTO.OriginID = "ce4d2959-cc81-4722-8801-eba55173536";
                payloadHeaderDTO.SessionID = "10";
                payloadHeaderDTO.ActionType = "chatAction";

                string testPayload = "testPayload";

                ObjectPayloadDTO objectPayloadDTO = new ObjectPayloadDTO();
                objectPayloadDTO.Header = payloadHeaderDTO;
                objectPayloadDTO.Payload = testPayload;

                string message = JsonConvert.SerializeObject(objectPayloadDTO);

                ws.Connect();
                ws.Send(message);

                Console.ReadKey();
            }
        }
    }
}
