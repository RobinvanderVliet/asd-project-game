using System.Collections.Generic;
using ASD_Game.InputHandling.Models;

namespace ASD_Game.InputHandling
{
    public interface IInputHandler
    {
        public void HandleGameScreenCommands();
        public void HandleStartScreenCommands();
        public void HandleSessionScreenCommands();
        public void HandleEditorScreenCommands();
        public string CustomRuleHandleEditorScreenCommands(string type);
        public bool CheckInput(List<string> rule, BaseVariables variables);
        public void HandleConfigurationScreenCommands();
        public void HandleLobbyScreenCommands();
    }
}