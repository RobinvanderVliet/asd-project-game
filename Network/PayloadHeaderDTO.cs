/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 5.
     
    Goal of this file: Format of sent message (header).
     
*/

namespace Network
{
    class PayloadHeaderDTO
    {
        public string target { get; set; }
        public string originID { get; set; }
        public string sessionID { get; set; }
        public string actionType { get; set; }

        public string payload { get; set; }
    }
}
