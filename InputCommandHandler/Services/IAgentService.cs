namespace InputCommandHandler
{
    public interface IAgentService
    {
        public void Activate();
        public void DeActivate();
        public bool IsActivated();
    }
}