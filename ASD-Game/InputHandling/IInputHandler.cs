namespace ASD_project.InputHandling
{
    public interface IInputHandler
    {
        public void HandleGameScreenCommands();
        public void HandleStartScreenCommands();
        public void HandleSessionScreenCommands();
        public void HandleConfigurationScreenCommands();
        public void HandleLobbyScreenCommands();
    }
}