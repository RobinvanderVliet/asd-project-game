using System.Collections.Generic;

namespace Session.DTO
{
    public class SessionDTO
    {
        public SessionType SessionType { get; set; }
        public string Name { get; set; }
        public List<string> ClientIds { get; set; }
        
        public int SessionSeed { get; set; }

        public SessionDTO(SessionType sessionType)
        {
            SessionType = sessionType;
        }
    }
}