using Player.Services;
using Session;
using UserInterface;

namespace InputCommandHandler
{
    public interface IInputCommandHandlerComponent
    {
        public void HandleCommands(IPlayerService playerService, ISessionService sessionService);
        public void HandleStartScreenCommands(IScreenHandler screenHandler, ISessionService sessionService);
    }
}