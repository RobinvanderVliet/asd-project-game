using Player.Services;
using Session;
using UserInterface;

namespace InputCommandHandler
{
    public interface IInputCommandHandlerComponent
    {
        public void HandleGameScreenCommands();
        public void HandleStartScreenCommands();
        public void HandleSessionScreenCommands();
        public void HandleEditorScreenCommands();
    }
}