using System.Collections.Generic;

namespace ASD_Game.UserInterface
{
    public interface IGameChatScreen : IScreen
    {
        public void ShowMessages(Queue<string> messages);
    }
}
