using System.Diagnostics.CodeAnalysis;

namespace Network.DTO
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
    }
}
