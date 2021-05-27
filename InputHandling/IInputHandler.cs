using Player.Services;
using Session;
using UserInterface;

namespace InputHandling
{
    public interface IInputHandler
    {
        public void HandleGameScreenCommands();
        public void HandleStartScreenCommands();
        public void HandleSessionScreenCommands();
    }
}