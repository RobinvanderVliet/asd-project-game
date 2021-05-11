namespace Session.DTO
{
    public class SessionDTO
    {
        public SessionType SessionType { get; set; }
        
        public string Name { get; set; }

        public SessionDTO(SessionType sessionType)
        {
            SessionType = sessionType;
        }
    }
}