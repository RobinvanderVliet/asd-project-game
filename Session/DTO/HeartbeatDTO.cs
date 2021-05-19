namespace Session.DTO
{
    public class HeartbeatDTO
    {
        public string Name { get; set; }
        public SessionType SessionType { get; set; }

        public HeartbeatDTO(SessionType sessionType)
        {
            SessionType = sessionType;
        }
    }
}