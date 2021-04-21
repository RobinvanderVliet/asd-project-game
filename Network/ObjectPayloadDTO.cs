/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 5 Pepijn van Erp.
     
    Goal of this file: Format of sent message.
     
*/

namespace Network
{
    class ObjectPayloadDTO
    {
        public PayloadHeaderDTO header { get; set; }
        public string payload { get; set; }
    }
}
