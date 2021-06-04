using System.Collections.Generic;
using InputHandling.Models;

namespace InputHandling
{
    public interface IInputHandler
    {
        public void HandleGameScreenCommands();
        public void HandleStartScreenCommands();
        public void HandleSessionScreenCommands();
        public void HandleEditorScreenCommands();
        public string CustomRuleHandleEditorScreenCommands(string type);
        public bool CheckInput(List<string> rule, BaseVariables variables);
    }
}