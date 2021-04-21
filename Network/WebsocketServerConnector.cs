using System;
using Newtonsoft.Json;
using WebSocketSharp;

/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 5 Pepijn van Erp.
     
    Goal of this file: Create connection with websocket server.
     
*/

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
                    Console.WriteLine("Received: " + objectPayloadDTO2.header.target);
                    Console.WriteLine("Received: " + objectPayloadDTO2.payload);

                    ObjectPayloadHandler objectPayloadHandler = new ObjectPayloadHandler();

                    if(objectPayloadHandler.checkHeader(objectPayloadDTO2.header))
                    {
                        objectPayloadHandler.checkActionType(objectPayloadDTO2);
                    }
                };

                PayloadHeaderDTO payloadHeaderDTO = new PayloadHeaderDTO();
                payloadHeaderDTO.target = "host";
                payloadHeaderDTO.originID = "ce4d2959-cc81-4722-8801-eba55173536";
                payloadHeaderDTO.sessionID = "10";
                payloadHeaderDTO.actionType = "chatAction";

                string testPayload = "testPayload";

                ObjectPayloadDTO objectPayloadDTO = new ObjectPayloadDTO();
                objectPayloadDTO.header = payloadHeaderDTO;
                objectPayloadDTO.payload = testPayload;

                string message = JsonConvert.SerializeObject(objectPayloadDTO);

                ws.Connect();
                ws.Send(message);

                Console.ReadKey();
            }
        }
    }
}
