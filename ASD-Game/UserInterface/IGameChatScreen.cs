using System.Collections.Generic;

namespace UserInterface
{
    public interface IGameChatScreen: IScreen
    {
        public void ShowMessages(Queue<string> messages);
    }
}
