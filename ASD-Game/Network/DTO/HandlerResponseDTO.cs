using System.Diagnostics.CodeAnalysis;
using ASD_Game.Network.Enum;

namespace ASD_Game.Network.DTO
{
    [ExcludeFromCodeCoverage]
    public class HandlerResponseDTO
    {
        public SendAction Action { get; set; }
        public string ResultMessage { get; set; }

        public HandlerResponseDTO(SendAction action, string resultMessage)
        {
            Action = action;
            ResultMessage = resultMessage;
        }

        public override bool Equals(object obj)
        {
            return obj is HandlerResponseDTO DTO &&
                   Action == DTO.Action &&
                   ResultMessage == DTO.ResultMessage;
        }
        public override int GetHashCode()
        {
            return Action.GetHashCode();
        }
    }
}
