using System;
using InputCommandHandler.Antlrr;
using InputCommandHandler.Exceptions;
using Player.Services;
using Session;

namespace InputCommandHandler
{
    public class InputCommandHandlerComponent
    {
        public void HandleCommands(IPlayerService playerService, ISessionService sessionService, IGamesSessionService gamesSessionService)
        {
            SendCommand(GetCommand(), playerService, sessionService, gamesSessionService);
        }
        private static void SendCommand(string commando, IPlayerService playerService, ISessionService sessionService, IGamesSessionService gamesSessionService)
        {
            try
            {
                var pipeline = new Pipeline();
                pipeline.ParseCommand(commando);
                pipeline.Transform(playerService, sessionService, gamesSessionService);
            }
            catch (CommandSyntaxException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (MoveException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public virtual string GetCommand()
        {
            return Console.ReadLine();
        }
    }
}