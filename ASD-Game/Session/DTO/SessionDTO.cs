using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Session.DTO
{
    [ExcludeFromCodeCoverage]
    public class SessionDTO
    {
        public SessionType SessionType { get; set; }
        public string Name { get; set; }
        public List<string[]> Clients { get; set; }
        
        public int SessionSeed { get; set; }

        public SessionDTO(SessionType sessionType)
        {
            SessionType = sessionType;
        }
        public SessionDTO()
        {

        }
        
        public bool SavedGame { get; set; }
    }
}