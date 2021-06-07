using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ASD_project.Session.DTO
{
    [ExcludeFromCodeCoverage]
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
        public SessionDTO()
        {

        }
    }
}