using Player.Services;
using Session;

namespace InputCommandHandler
{
    public interface IInputCommandHandlerComponent
    {
        public void HandleGameScreenCommands();
        public void HandleStartScreenCommands();
        public void HandleSessionScreenCommands();
        public void HandleConfigurationScreenCommands();
    }
}