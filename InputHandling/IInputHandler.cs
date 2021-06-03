namespace InputHandling
{
    public interface IInputHandler
    {
        public void HandleGameScreenCommands();
        public void HandleStartScreenCommands();
        public void HandleSessionScreenCommands();
        public void HandleConfigurationScreenCommands();
    }
}